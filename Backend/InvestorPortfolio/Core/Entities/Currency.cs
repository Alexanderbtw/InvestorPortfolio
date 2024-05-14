using System.Diagnostics.CodeAnalysis;
using Core.Entities.Base;
using Core.Entities.SpecificData;

namespace Core.Entities;

[method: SetsRequiredMembers]
public class Currency() : Stock
{
    public required CurrencyCode OriginalCurrency { get; init; }
}