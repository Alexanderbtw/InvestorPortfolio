using Core.Entities.SpecificData;

namespace Infrastructure.DTOs;

public class CurrencyDataRequest
{
    public required Dictionary<CurrencyCode, MoneyValue> Data { get; init; }
}