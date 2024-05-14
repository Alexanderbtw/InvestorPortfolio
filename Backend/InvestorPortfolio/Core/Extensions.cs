using Core.Entities.SpecificData;

namespace Core;

public static class Extensions
{
    public static MoneyValue Sum(this IEnumerable<MoneyValue> source) 
    {
        ulong units = 0;
        uint nano = 0;
        foreach (var item in source) 
        {
            if (item.Currency != source.First().Currency) throw new ArgumentException("Currencies are not equal");

            units += item.Units;
            nano += item.Nano;
            units += nano / 1_000_000_000;
            nano %= 1_000_000_000;
        }

        return new MoneyValue()
        {
            Currency = source.First().Currency,
            Units = units,
            Nano = nano
        };
    }
}