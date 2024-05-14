using System.Net.Http.Json;
using System.Text.Json;
using Application.Interfaces;
using Core.Entities.Base;
using Core.Entities.SpecificData;
using Infrastructure.DTOs;
using Infrastructure.Interfaces;

namespace Infrastructure.Converters.CurrencyApi;

public class CurrencyConverter : ICurrencyConverter<MoneyValue, CurrencyCode>, IAsyncInitialization
{
    private readonly CurrencyConvertApiClient _client;
    private Dictionary<CurrencyCode, MoneyValue>? _exchangeRatesUSD;
    public CurrencyConverter(CurrencyConvertApiClient client)
    {
        _client = client;
        Initialization = InitializeAsync();
    }

    private async Task InitializeAsync()
    {
        const string uri = "/v3/latest/"; // $"?base_currency={moneyFrom.Currency.Value}&currencies={toCurrency.Value}"

        var result = await _client.Client.GetFromJsonAsync<CurrencyDataRequest>(uri, options: new JsonSerializerOptions()
        {
            Converters = { new CustomCurrencyApiDataConverter() }
        });
        _exchangeRatesUSD = result?.Data;
    }

    public Task Initialization { get; }

    public async Task<MoneyValue?> ConvertAsync(MoneyValue moneyFrom, CurrencyCode toCurrency, CancellationToken cancellationToken = default)
    {
        await Initialization;
        
        if (_exchangeRatesUSD == null) throw new NullReferenceException();
        var value = 
            MoneyValue.FromDecimal(
                moneyFrom.ToDecimal() / _exchangeRatesUSD[moneyFrom.Currency].ToDecimal() * _exchangeRatesUSD[toCurrency].ToDecimal(), 
                toCurrency);

        return value;
    }
}