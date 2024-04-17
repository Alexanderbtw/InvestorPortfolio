using Core.Entities.SpecificData;

namespace Core.DTOs;

public class CurrencyDataRequest
{
    public Dictionary<CurrencyCode, MoneyValue> Data { get; init; }
}