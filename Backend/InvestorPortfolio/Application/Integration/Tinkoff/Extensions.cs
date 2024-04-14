using System.Reflection;
using Application.Integration.Tinkoff.MappingProfiles;
using Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Tinkoff.InvestApi;

namespace Application.Integration.Tinkoff;

public static class Extensions
{
    public static IServiceCollection AddInvestmentTinkoffClient(this IServiceCollection services, Action<IServiceProvider, InvestApiSettings> configure)
    {
        services.AddAutoMapper(typeof(TinkoffDatasProfile).Assembly);
        services.AddInvestApiClient(configure);
        services.AddScoped<IStockExchange, TinkoffStockExchange>();
        return services;
    }
}