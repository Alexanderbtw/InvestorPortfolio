using Application.Integrations.Tinkoff;
using Core.Entities.Base;
using Core.Entities.SpecificData;
using Infrastructure.Converters.CurrencyApi;
using Infrastructure.Interfaces;
using Persistence.FileSavers;
using Persistence.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string? token = builder.Configuration["ThirdParty:TOKEN"];
ArgumentException.ThrowIfNullOrEmpty(token, nameof(token));
string? apikey = builder.Configuration["ThirdParty:APIKEY"];
ArgumentException.ThrowIfNullOrEmpty(apikey, nameof(apikey));

builder.Services.AddInvestmentTinkoffClient((provider, settings) =>
{
    settings.AccessToken = token;
});
builder.Services.AddHttpClient<CurrencyConvertApiClient, CurrencyApiClient>(httpClient => {
    httpClient.DefaultRequestHeaders.Add("apikey", apikey);
});
builder.Services.AddTransient<ICurrencyConverter<MoneyValue, CurrencyCode>, CurrencyConverter>();
builder.Services.AddSingleton(typeof(IAsyncFileSaver<>), typeof(JsonSaver<>)); 
builder.Services.AddSingleton(typeof(IFileSaver<>), typeof(XmlSaver<>));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("/portfolio", () =>
    {
        
    })
    .WithName("GetPortfolio")
    .WithOpenApi();

app.Run();