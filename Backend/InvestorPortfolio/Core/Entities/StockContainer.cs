using Core.Entities.Base;
using Core.Entities.SpecificData;

namespace Core.Interfaces;

public class StockContainer<T> : IEquatable<StockContainer<T>> where T : Stock
{
    public long Quantity { get; private set; }
    
    public DateTime PurchaseDate { get; init; }
    public MoneyValue OnePurchasePrice { get; private set; }
    public T Stock { get; init; }
    public decimal SumPurchasePrice => Quantity * OnePurchasePrice.Units + ((int)Quantity * OnePurchasePrice.Nano / 1_000_000_000);

    public StockContainer(T stock, long quantity)
    {
        Stock = stock;
        Quantity = quantity * stock.Lot;
        PurchaseDate = DateTime.Today;
        OnePurchasePrice = stock.Nominal;
    }

    public void AddStock(T stock, long quantity)
    {
        if (!Stock.Equals(stock) || PurchaseDate != DateTime.Today) throw new NotEqualStockException("Stocks are not equal");

        quantity *= stock.Lot;
        Quantity += quantity;

        var extUnits = (OnePurchasePrice.Nano + stock.Nominal.Nano) / Quantity / 1_000_000_000;
        var nanos = (OnePurchasePrice.Nano + stock.Nominal.Nano) / Quantity % 1_000_000_000;
        OnePurchasePrice = new MoneyValue(
            stock.Nominal.Currency, 
            (OnePurchasePrice.Units + stock.Nominal.Units) / Quantity + extUnits, 
            (int)nanos
        );
    }

    public bool TryRemoveStock(long quantity, out bool isZero)
    {
        isZero = false;
        if (Quantity < quantity)
        {
            return false;
        } 
        else if (Quantity == quantity) isZero = true;
        Quantity -= quantity;
        return true;
    }
    
    public decimal GetTotalValue() => Quantity * OnePurchasePrice.Nano + Quantity * OnePurchasePrice.Units;

    public bool Equals(StockContainer<T>? other)
    {
        return PurchaseDate == other?.PurchaseDate && Stock.Equals(other.Stock);
    }
}