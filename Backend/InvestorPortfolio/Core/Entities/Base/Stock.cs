using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Core.Entities.SpecificData;

namespace Core.Entities.Base;

[ComplexType]
[method: SetsRequiredMembers]
public abstract class Stock() : IEquatable<Stock>
{
    public required string? Isin { get; init; } = string.Empty;

    public required string? Uid { get; init; } = string.Empty;
    public required string? Ticker { get; init; } = string.Empty;
    public required MoneyValue Nominal { get; init; } = MoneyValue.Zero;
    public required ulong Lot { get; init; } = 1;
    public required string? Name { get; init; } = string.Empty;

    public bool Equals(Stock? other)
    {
        if (other is null) return false;
        
        return Isin == other.Isin && Nominal.Currency.Equals(other.Nominal.Currency);
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as Stock);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Isin, Nominal);
    }
}