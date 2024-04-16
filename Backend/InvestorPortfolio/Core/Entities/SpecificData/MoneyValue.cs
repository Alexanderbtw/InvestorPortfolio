namespace Core.Entities.SpecificData;

public record struct MoneyValue(CurrencyCode Currency, long Units, int Nano) 
{
    public static MoneyValue operator +(MoneyValue left, MoneyValue right) 
    {
        if (left.Currency != right.Currency) throw new ArgumentException("Currencies are not equal");

        return new(left.Currency, left.Units + right.Units + ((left.Nano + right.Nano) / 1_000_000_000), (left.Nano + right.Nano) % 1_000_000_000);
    }
}