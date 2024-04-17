using Core.Entities.Base;
using Core.Entities.SpecificData;

namespace Core.Entities;

public class Currency : Stock
{
    public CurrencyCode SettlementCurrency { get; init; }

    public Currency(string isin, string ticker, MoneyValue nominal, ulong lot, string name, CurrencyCode? settlementCurrency = null) : base(isin, ticker, nominal, lot, name)
    {
        SettlementCurrency = settlementCurrency ?? nominal.Currency;
    }
}