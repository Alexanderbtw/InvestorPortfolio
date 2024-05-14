using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Core.Entities.Base;
using Core.Entities.SpecificData;

namespace Core.Entities;

public class StockContainer<T> : IEquatable<StockContainer<T>> where T : Stock
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public ulong Count { get; private set; }
    
    public DateOnly PurchaseDate { get; init; } = DateOnly.FromDateTime(DateTime.Today);
    [Required] public MoneyValue OnePurchaseValue { get; private set; }
    
    [Required] public T LastStock { get; private set; }

    [NotMapped] public MoneyValue SumPurchaseValue =>
        new MoneyValue()
        {
            Currency = OnePurchaseValue.Currency,
            Units = Count * OnePurchaseValue.Units + Count * OnePurchaseValue.Nano / 1_000_000_000,
            Nano = (uint)(Count * OnePurchaseValue.Nano % 1_000_000_000)
        };
    [NotMapped] public decimal SumPurchasePrice => 
        Count * OnePurchaseValue.ToDecimal();

    private StockContainer() { }
    
    [SetsRequiredMembers] 
    public StockContainer(T stock, ulong quantity)
    {
        LastStock = stock;
        Count = quantity * stock.Lot;
        PurchaseDate = DateOnly.FromDateTime(DateTime.Today);
        OnePurchaseValue = stock.Nominal;
    }
    
    [JsonConstructor]
    public StockContainer(T lastStock, ulong count, DateOnly purchaseDate, MoneyValue onePurchaseValue)
    {
        LastStock = lastStock;
        Count = count;
        PurchaseDate = purchaseDate;
        OnePurchaseValue = onePurchaseValue;
    }

    public void AddStock(T stock, ulong quantity)
    {
        if (!LastStock.Equals(stock) || PurchaseDate !=  DateOnly.FromDateTime(DateTime.Today)) throw new NotEqualStockException("Stocks are not equal");
        
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