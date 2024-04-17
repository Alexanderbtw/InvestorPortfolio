using Core.Entities.Base;
using Core.Entities.SpecificData;

namespace Core.Entities;

public class Share : Stock
{
    public ShareType Type{ get; set; }

    public Share(string isin, string ticker, MoneyValue nominal, ulong lot, string name, ShareType type = ShareType.Unspecified) : base(isin, ticker, nominal, lot, name)
    {
        Type = type;
    }
}

public enum ShareType
{
    /// <summary>Значение не определено.</summary>
    Unspecified,
    /// <summary>Обыкновенная</summary>
    Common,
    /// <summary>Привилегированная</summary>
    Preferred,
    /// <summary>Американские депозитарные расписки</summary>
    Adr,
    /// <summary>Глобальные депозитарные расписки</summary>
    Gdr,
    /// <summary>Товарищество с ограниченной ответственностью</summary>
    Mlp,
    /// <summary>Акции из реестра Нью-Йорка</summary>
    NyRegShrs,
    /// <summary>Закрытый инвестиционный фонд</summary>
    ClosedEndFund,
    /// <summary>Траст недвижимости</summary>
    Reit,
}