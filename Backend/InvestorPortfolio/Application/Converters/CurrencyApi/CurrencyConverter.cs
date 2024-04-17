using System.Net.Http.Json;
using System.Text.Json;
using Core.DTOs;
using Core.Entities.Base;
using Core.Entities.SpecificData;

namespace Application.Converters.CurrencyApi;

public class CurrencyConverter : ICurrencyConverter<MoneyValue, CurrencyCode>
{
    private readonly CurrencyConvertApiClient _client;

    public CurrencyConverter(CurrencyConvertApiClient client)
    {
        _client = client;
    }

    public async Task<MoneyValue> ConvertAsync(MoneyValue moneyFrom, CurrencyCode toCurrency, CancellationToken cancellationToken = default)
    {
        var uri = "/v3/latest/" + $"?base_currency={moneyFrom.Currency.Value}&currencies={toCurrency.Value}";
        
        var result = await _client.Client.GetFromJsonAsync<CurrencyDataRequest>(uri, options: new JsonSerializerOptions()
        {
            Converters = { new CustomCurrencyApiDataConverter() }
        }, cancellationToken);
        var value = MoneyValue.FromDecimal(moneyFrom.ToDecimal() * result.Data[toCurrency].ToDecimal(), toCurrency);

        return value;
    }
}