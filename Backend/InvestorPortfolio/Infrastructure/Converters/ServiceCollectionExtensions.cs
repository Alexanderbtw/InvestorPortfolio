using Application.Interfaces;
using Core.Entities.SpecificData;
using Infrastructure.Converters.CurrencyApi;
using Infrastructure.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Converters;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCurrencyConverter(this IServiceCollection services, string apikey)
    {
        services.AddHttpClient<CurrencyConvertApiClient, CurrencyApiClient>(httpClient => {
            httpClient.DefaultRequestHeaders.Add("apikey", apikey);
        });
        services.AddTransient<ICurrencyConverter<MoneyValue, CurrencyCode>, CurrencyConverter>();
        
        return services;
    }
}