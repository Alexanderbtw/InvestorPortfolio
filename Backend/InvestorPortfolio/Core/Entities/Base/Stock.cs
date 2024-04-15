using Core.Entities.SpecificData;

namespace Core.Entities.Base;

public abstract class Stock : IEquatable<Stock>
{
    public string Isin { get; init; } = string.Empty;
    public string Ticker { get; init; } = string.Empty;
    public MoneyValue Nominal { get; init; }
    public long Lot { get; init; }
    public string Name { get; init; } = string.Empty;

    public bool Equals(Stock? other)
    {
        if (other is null) return false;
        
        return Isin == other.Isin && Nominal.Currency == other.Nominal.Currency;
    }
}