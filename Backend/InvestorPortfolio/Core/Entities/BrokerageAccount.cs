using System.Text.Json.Serialization;
using Core.Interfaces;

namespace Core.Entities;

public class BrokerageAccountBuilder : IBuilder<BrokerageAccount>
{
    private readonly Portfolio _owner; 
    private HashSet<StockContainer<Share>> _shares;
    private HashSet<StockContainer<Bond>> _bonds;
    private HashSet<StockContainer<Currency>> _currencies;
    private readonly string _title;
    public BrokerageAccountBuilder(Portfolio owner, string title)
    {
        _owner = owner;
        _title = title;
    }
    
    public BrokerageAccountBuilder WithShares(HashSet<StockContainer<Share>> shares)
    {
        _shares = shares;
        return this;
    }
    
    public BrokerageAccountBuilder WithBonds(HashSet<StockContainer<Bond>> bonds)
    {
        _bonds = bonds;
        return this;
    }
    
    public BrokerageAccountBuilder WithCurrencies(HashSet<StockContainer<Currency>> currencies)
    {
        _currencies = currencies;
        return this;
    }
    
    public BrokerageAccount Build()
    {
        return new BrokerageAccount(_owner, _title, _shares, _bonds, _currencies);
    }
}

public class BrokerageAccount
{
    public Guid Id { get; init; }
    public string Title { get; init; } = string.Empty;
    
    [JsonIgnore]
    public Portfolio Owner { get; init; }
    public HashSet<StockContainer<Share>> Shares { get; init; } = [];
    public HashSet<StockContainer<Bond>> Bonds { get; init; } = [];
    public HashSet<StockContainer<Currency>> Currencies { get; init; } = [];
    
    public BrokerageAccount(Portfolio owner, string title)
    {
        Owner = owner;
        Title = title;
    }
    
    public BrokerageAccount(Portfolio owner, string title, HashSet<StockContainer<Share>>? shares, HashSet<StockContainer<Bond>>? bonds, HashSet<StockContainer<Currency>>? currencies)
    {
        Title = title;
        Owner = owner;
        Shares = shares ?? Shares;
        Bonds = bonds ?? Bonds;
        Currencies = currencies ?? Currencies;
    }

    public void AddShares(Share share, long quantity) 
    {
        Shares.Add(new StockContainer<Share>(share, quantity));
    }

    public void AddBonds(Bond bond, long quantity) 
    {
        Bonds.Add(new StockContainer<Bond>(bond, quantity));
    }

    public void AddCurrencies(Currency currency, long quantity)
    {
        Currencies.Add(new StockContainer<Currency>(currency, quantity));
    }

    public bool TryRemoveShares(Share share, long quantity)
    {
        var shares = new StockContainer<Share>(share, quantity);
        bool isZero = false;
        bool result = Shares.FirstOrDefault(x => x.Equals(shares))?.TryRemoveStock(quantity, out isZero) ?? false;
        if (isZero) Shares.Remove(shares);
        return result;
    }

    public bool TryRemoveBonds(Bond bond, long quantity)
    {
        var bonds = new StockContainer<Bond>(bond, quantity);
        bool isZero = false;
        bool result = Bonds.FirstOrDefault(x => x.Equals(bonds))?.TryRemoveStock(quantity, out isZero) ?? false;
        if (isZero) Bonds.Remove(bonds);
        return result;
    }

    public bool TryRemoveCurrencies(Currency currency, long quantity)
    {
        var currencies = new StockContainer<Currency>(currency, quantity);
        bool isZero = false;
        bool result = Currencies.FirstOrDefault(x => x.Equals(currencies))?.TryRemoveStock(quantity, out isZero) ?? false;
        if (isZero) Currencies.Remove(currencies);
        return result;
    }
}