using Core.Entities.SpecificData;

public class CurrencyConverter : ICurrencyConverter<MoneyValue, CurrencyCode>
{
    private readonly ICurrencyConvertApiClient _client;

    public CurrencyConverter(ICurrencyConvertApiClient client)
    {
        _client = client;
    }

    public async Task<MoneyValue> ConvertAsync(MoneyValue moneyFrom, CurrencyCode toCurrency)
    {
        return await Task.FromResult(moneyFrom);
    }
}