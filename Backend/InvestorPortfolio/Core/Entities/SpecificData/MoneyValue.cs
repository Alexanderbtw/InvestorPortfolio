using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Core.Entities.SpecificData;

[ComplexType]
public class MoneyValue
{
    public CurrencyCode Currency { get; init; }
    public ulong Units { get; init; }
    public uint Nano { get; init; }
    
    public static MoneyValue operator +(MoneyValue left, MoneyValue right)
    {
        if (left.Currency != right.Currency) throw new ArgumentException("Currencies are not equal");

        return new MoneyValue()
        {
            Currency = left.Currency,
            Units = left.Units + right.Units + ((left.Nano + right.Nano) / 1_000_000_000),
            Nano = (left.Nano + right.Nano) % 1_000_000_000
        };
    }

    public static MoneyValue operator -(MoneyValue left, MoneyValue right)
    {
        if (left.Currency != right.Currency) throw new ArgumentException("Currencies are not equal");
        if (left.Nano < right.Nano && left.Units <= right.Units) throw new ArgumentException("Not enough value");

        return new MoneyValue
        {
            Currency = left.Currency,
            Units = left.Units - right.Units - ((left.Nano - right.Nano) / 1_000_000_000),
            Nano = (left.Nano - right.Nano) % 1_000_000_000
        };
    }

    public decimal ToDecimal() => Units + Nano / 1_000_000_000m;

    public static MoneyValue? FromDecimal(decimal value, CurrencyCode currency) =>
        new()
        {
            Currency = currency,
            Units = (ulong)value,
            Nano = (uint)((value - (ulong)value) * 1_000_000_000)
        };
    
    public static MoneyValue Zero => new()
    {
        Currency = new CurrencyCode("UNSPECIFIED"),
        Units = 0,
        Nano = 0
    };

    public void Deconstruct(out CurrencyCode Currency, out ulong Units, out uint Nano)
    {
        Currency = this.Currency;
        Units = this.Units;
        Nano = this.Nano;
    }
}