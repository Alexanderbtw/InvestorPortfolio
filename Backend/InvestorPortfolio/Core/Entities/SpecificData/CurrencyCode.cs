using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Core.Entities.SpecificData;

[ComplexType]
public class CurrencyCode : IEquatable<CurrencyCode>
{
    public CurrencyCode() { }
    public CurrencyCode(string value)
    {
        Value = value;
    }

    public string Value { get; init; } = string.Empty;

    public void Deconstruct(out string value)
    {
        value = this.Value;
    }

    public bool Equals(CurrencyCode? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Value == other.Value;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((CurrencyCode)obj);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
}