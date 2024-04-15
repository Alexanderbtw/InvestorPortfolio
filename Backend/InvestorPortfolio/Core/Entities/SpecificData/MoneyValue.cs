namespace Core.Entities.SpecificData;

public record struct MoneyValue(string Currency, long Units, int Nano);