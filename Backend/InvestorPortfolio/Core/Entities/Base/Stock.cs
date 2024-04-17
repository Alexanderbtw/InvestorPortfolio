using Core.Entities.SpecificData;

namespace Core.Entities.Base;

public abstract class Stock : IEquatable<Stock>
{
    public string Isin { get; init; }
    public string Ticker { get; init; }
    public MoneyValue Nominal { get; init; }
    public ulong Lot { get; init; }
    public string Name { get; init; }

    protected Stock(string isin, string ticker, MoneyValue nominal, ulong lot, string name)
    {
        Isin = isin;
        Ticker = ticker;
        Nominal = nominal;
        Lot = lot;
        Name = name;
    }

    public bool Equals(Stock? other)
    {
        if (other is null) return false;
        
        return Isin == other.Isin && Nominal.Currency == other.Nominal.Currency;
    }
}