using Core.Entities.Base;

namespace Core.Entities;

public class Share : Stock
{
    public ShareType Type{ get; set; }
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