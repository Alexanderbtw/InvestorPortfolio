using Core.Entities.Base;
using Core.Entities.SpecificData;

namespace Core.Entities;

public class Currency : Stock
{
    public string SettlementCurrency { get; init; } = string.Empty;
}