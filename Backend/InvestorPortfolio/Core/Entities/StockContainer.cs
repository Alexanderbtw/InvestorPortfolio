using Core.Entities.Base;
using Core.Entities.SpecificData;

namespace Core.Entities;

public class StockContainer<T> : IEquatable<StockContainer<T>> where T : Stock
{
    public ulong Count { get; private set; }
    
    public DateTime PurchaseDate { get; init; }
    public MoneyValue OnePurchaseValue { get; private set; }
    public T LastStock { get; private set; }

    public MoneyValue SumPurchaseValue => 
        new(OnePurchaseValue.Currency, OnePurchaseValue.Units * Count + OnePurchaseValue.Nano * Count / 1_000_000_000, (uint)(OnePurchaseValue.Nano * Count % 1_000_000_000));
    public decimal SumPurchasePrice => 
        Count * OnePurchaseValue.ToDecimal();

    public StockContainer(T stock, ulong quantity)
    {
        LastStock = stock;
        Count = quantity * stock.Lot;
        PurchaseDate = DateTime.Today;
        OnePurchaseValue = stock.Nominal;
    }

    public void AddStock(T stock, ulong quantity)
    {
        if (!LastStock.Equals(stock) || PurchaseDate != DateTime.Today) throw new NotEqualStockException("Stocks are not equal");
        
        ulong count = stock.Lot * quantity;
        var currSumPurchaseValue = SumPurchaseValue;
        Count += count;
        
        decimal onePurchasePrice = (currSumPurchaseValue.Units + stock.Nominal.Units * count + (decimal)(currSumPurchaseValue.Nano + stock.Nominal.Nano * count) / 1_000_000_000) / Count;
        
        OnePurchaseValue = MoneyValue.FromDecimal(onePurchasePrice, stock.Nominal.Currency);
        
        LastStock = stock;
    }

    public bool TryRemoveStock(ulong quantity, ulong lot, out bool isZero)
    {
        quantity *= lot;
        isZero = false;
        if (Count < quantity)
        {
            return false;
        } 
        else if (Count == quantity) isZero = true;
        Count -= quantity;
        return true;
    }

    public bool Equals(StockContainer<T>? other)
    {
        return PurchaseDate == other?.PurchaseDate && LastStock.Equals(other.LastStock);
    }
}