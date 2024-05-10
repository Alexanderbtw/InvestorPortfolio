namespace Infrastructure.Interfaces;

public interface ICurrencyConverter<TValue, in TCurrency>
{
    Task<TValue> ConvertAsync(TValue moneyFrom, TCurrency toCurrency, CancellationToken cancellationToken = default);
}