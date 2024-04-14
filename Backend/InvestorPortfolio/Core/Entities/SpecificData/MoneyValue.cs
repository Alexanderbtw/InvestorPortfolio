namespace Core.Entities.SpecificData;

public record struct MoneyValue(string Currency, ulong Units, ulong Nano);