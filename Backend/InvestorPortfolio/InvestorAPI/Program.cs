using System.Text;
using Application.Integrations.Tinkoff;
using Application.Interfaces;
using Application.Services;
using Infrastructure.Auth;
using Infrastructure.Converters;
using Infrastructure.DTOs;
using InvestorAPI.Endpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Persistence;
using Persistence.FileSavers;
using Persistence.Repositories;
using Serilog;
using Serilog.Events;
using SerilogTracing;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft.AspNetCore.Hosting", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore.Routing", LogEventLevel.Warning)
    .Enrich.WithProperty("Application", "InvestorAPI")
    .WriteTo.Console()
    .WriteTo.Seq(
        "http://localhost:5341",
        apiKey: Environment.GetEnvironmentVariable("SEQ_API_KEY"))
    .CreateLogger();

using var listener = new ActivityListenerConfiguration()
    .Instrument.AspNetCoreRequests()
    .TraceToSharedLogger();

Log.Information("Starting up");

try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Services.AddSerilog();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    string? token = builder.Configuration["Tinkoff:TOKEN"];
    ArgumentException.ThrowIfNullOrEmpty(token, nameof(token));
    string? apikey = builder.Configuration["Exchange:APIKEY"];
    ArgumentException.ThrowIfNullOrEmpty(apikey, nameof(apikey));
    builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));

    builder.Services.AddDbContext<InvestmentDbContext>(options =>
    {
        options.UseNpgsql(builder.Configuration.GetConnectionString(nameof(InvestmentDbContext)));
    });

    builder.Services.AddInvestmentTinkoffClient((provider, settings) => { settings.AccessToken = token; });
    builder.Services.AddCurrencyConverter(apikey);
    builder.Services.AddFileSavers();
    builder.Services.AddApplication();

    var jwtOptions = new JwtOptions();
    builder.Configuration.GetSection(nameof(JwtOptions)).Bind(jwtOptions);
    builder.Services.AddApiAuthentication(jwtOptions);

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseCookiePolicy(new CookiePolicyOptions
    {
        MinimumSameSitePolicy = SameSiteMode.Strict,
        HttpOnly = HttpOnlyPolicy.Always,
        Secure = CookieSecurePolicy.Always
    });

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapGroup("/public").MapPublicInvestmentEndpoints()
        .WithOpenApi()
        .WithTags("Public");
    app.MapGroup("/auth").MapUserEndpoints()
        .WithOpenApi()
        .WithTags("Auth");
    app.MapGroup("/individual").MapIndividualInvestmentEndpoints()
        .RequireAuthorization()
        .WithOpenApi()
        .WithTags("Individual");

    app.Run();
}
catch (Exception e)
{
    Log.Fatal(e, "Unhandled exception");
    return 1;
}
finally
{
    await Log.CloseAndFlushAsync();
}
return 0;

internal static class Extensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IJwtProvider, JwtProvider>();
        services.AddScoped<UserService>();
        services.AddScoped<PortfolioService>();
        services.AddScoped<AccountService>();

        return services;
    }

    public static IServiceCollection AddApiAuthentication(this IServiceCollection services, JwtOptions jwtOptions)
    {
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new()
                {
                    NameClaimType = "userId",

                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtOptions.SecretKey))
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies["token"];
                        return Task.CompletedTask;
                    }
                };
            });

        services
            .AddAuthorizationBuilder()
            .AddPolicy("User", policy => policy.RequireClaim("userId"));

        return services;
    }
}