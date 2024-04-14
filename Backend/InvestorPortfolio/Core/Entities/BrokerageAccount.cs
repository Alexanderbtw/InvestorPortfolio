using System.Text.Json.Serialization;
using Core.Interfaces;

namespace Core.Entities;

public class BrokerageAccountBuilder : IBuilder<BrokerageAccount>
{
    private readonly Portfolio _owner; 
    private HashSet<Share> _shares;
    private HashSet<Bond> _bonds;
    private HashSet<Currency> _currencies;
    private readonly string _title;
    public BrokerageAccountBuilder(Portfolio owner, string title)
    {
        _owner = owner;
        _title = title;
    }
    
    public BrokerageAccountBuilder WithShares(HashSet<Share> shares)
    {
        _shares = shares;
        return this;
    }
    
    public BrokerageAccountBuilder WithBonds(HashSet<Bond> bonds)
    {
        _bonds = bonds;
        return this;
    }
    
    public BrokerageAccountBuilder WithCurrencies(HashSet<Currency> currencies)
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
    public HashSet<Share> Shares { get; init; } = [];
    public HashSet<Bond> Bonds { get; init; } = [];
    public HashSet<Currency> Currencies { get; init; } = [];
    
    public BrokerageAccount(Portfolio owner, string title)
    {
        Owner = owner;
        Title = title;
    }
    
    public BrokerageAccount(Portfolio owner, string title, HashSet<Share>? shares, HashSet<Bond>? bonds, HashSet<Currency>? currencies)
    {
        Title = title;
        Owner = owner;
        Shares = shares ?? Shares;
        Bonds = bonds ?? Bonds;
        Currencies = currencies ?? Currencies;
    }
}