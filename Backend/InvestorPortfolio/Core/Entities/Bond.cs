using Core.Entities.Base;
using Core.Entities.SpecificData;

namespace Core.Entities;

public class Bond : Stock
{
    public Bond(string isin, string ticker, MoneyValue nominal, ulong lot, string name) : base(isin, ticker, nominal, lot, name)
    {
        
    }
}