using Core.Entities.SpecificData;

public interface ICurrencyConverter<TValue, TypeCurrency>
{
    Task<TValue> ConvertAsync(TValue moneyFrom, TypeCurrency toCurrency, CancellationToken cancellationToken = default);
}