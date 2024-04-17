namespace Core.Entities.SpecificData;

public record struct MoneyValue(CurrencyCode Currency, ulong Units, uint Nano)
{
    public static MoneyValue operator +(MoneyValue left, MoneyValue right)
    {
        if (left.Currency != right.Currency) throw new ArgumentException("Currencies are not equal");

        return new MoneyValue(left.Currency, left.Units + right.Units + ((left.Nano + right.Nano) / 1_000_000_000),
            (left.Nano + right.Nano) % 1_000_000_000);
    }

    public static MoneyValue operator -(MoneyValue left, MoneyValue right)
    {
        if (left.Currency != right.Currency) throw new ArgumentException("Currencies are not equal");

        return new MoneyValue(left.Currency, left.Units - right.Units - ((left.Nano - right.Nano) / 1_000_000_000),
            (left.Nano - right.Nano) % 1_000_000_000);
    }

    public decimal ToDecimal() => Units + Nano / 1_000_000_000m;

    public static MoneyValue FromDecimal(decimal value, CurrencyCode currency) =>
        new(currency, (ulong)value, (uint)((value - (ulong)value) * 1_000_000_000));
}