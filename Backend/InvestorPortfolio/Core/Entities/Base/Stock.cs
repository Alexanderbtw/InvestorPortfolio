using Core.Entities.SpecificData;

namespace Core.Entities.Base;

public abstract class Stock
{
    public string Ticker { get; init; } = string.Empty;
    public MoneyValue Nominal { get; init; }
    public int Lot { get; init; }
    public string Name { get; init; } = string.Empty;
}