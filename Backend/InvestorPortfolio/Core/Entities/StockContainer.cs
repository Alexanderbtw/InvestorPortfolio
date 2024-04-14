using Core.Entities.Base;
using Core.Entities.SpecificData;

namespace Core.Interfaces;

public class StockContainer<T> where T : Stock
{
    public ulong Quantity { get; private set; }
    
    public DateTime DateTimePurchase { get; init; }
    public MoneyValue OnePurchasePrice { get; private set; }
    public T Stock { get; init; }

    public StockContainer(T stock, ulong quantity)
    {
        Stock = stock;
        Quantity = quantity;
        DateTimePurchase = DateTime.Today;
        OnePurchasePrice = stock.Nominal;
    }

    public void AddStock(T Stock, ulong quantity)
    {
        var ratio = Quantity / quantity;
        Quantity += quantity;
        OnePurchasePrice = new MoneyValue(OnePurchasePrice.Currency, OnePurchasePrice.Units * ratio, OnePurchasePrice.Nano * ratio);
    }

    public void RemoveStock(ulong quantity)
    {
        if (Quantity < quantity)
        {
            throw new Exception("Not enough quantity");
        }
        Quantity -= quantity;
    }
    
    public decimal GetTotalValue() => Quantity * OnePurchasePrice.Nano + Quantity * OnePurchasePrice.Units;
    
    
}