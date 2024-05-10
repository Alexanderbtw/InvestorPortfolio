using Application.Integration.Tinkoff;
using Application.Integrations.Tinkoff.MappingProfiles;
using Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Tinkoff.InvestApi;

namespace Application.Integrations.Tinkoff;

public static class Extensions
{
    public static IServiceCollection AddInvestmentTinkoffClient(this IServiceCollection services, Action<IServiceProvider, InvestApiSettings> configure)
    {
        services.AddAutoMapper(typeof(TinkoffDatasProfile).Assembly);
        services.AddInvestApiClient(configure);
        services.AddScoped<IStockExchange, TinkoffStockExchangeService>();
        return services;
    }
}