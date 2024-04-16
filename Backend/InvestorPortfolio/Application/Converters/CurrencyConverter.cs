using Core.Entities.Base;
using Core.Entities.SpecificData;
using Core.Interfaces;

namespace Application.Converters;

public class CurrencyConverter : ICurrencyConverter<MoneyValue, CurrencyCode>
{
    private readonly CurrencyConvertApiClient _client;

    public CurrencyConverter(CurrencyConvertApiClient client)
    {
        _client = client;
    }

    public async Task<MoneyValue> ConvertAsync(MoneyValue moneyFrom, CurrencyCode toCurrency)
    {
        return await Task.FromResult(moneyFrom);
    }
}