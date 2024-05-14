using Application.Integrations.Tinkoff.MappingProfiles;
using Application.Services.StockExchanges;
using Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Tinkoff.InvestApi;

namespace Application.Integrations.Tinkoff;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInvestmentTinkoffClient(this IServiceCollection services, Action<IServiceProvider, InvestApiSettings> configure)
    {
        services.AddAutoMapper(typeof(TinkoffDataProfile).Assembly);
        services.AddInvestApiClient(configure);
        services.AddScoped<IStockExchangeService, TinkoffStockExchangeServiceService>();
        return services;
    }
}