using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Core.Entities.Base;
using Core.Entities.SpecificData;
using Core.Interfaces;

namespace Core.Entities;

public class BrokerageAccountBuilder(Portfolio owner, string title) : IBuilder<BrokerageAccount>
{
    private HashSet<StockContainer<Share>>? _shares;
    private HashSet<StockContainer<Bond>>? _bonds;
    private HashSet<StockContainer<Currency>>? _currencies;

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
        return new BrokerageAccount
        {
            Owner = owner,
            Title = title,
            Bonds = _bonds ?? [],
            Currencies = _currencies ?? [],
            Shares = _shares ?? [],
        };
    }
}

public class BrokerageAccount : IEquatable<BrokerageAccount>
{
    public BrokerageAccount() { }

    [JsonConstructor]
    public BrokerageAccount(Guid id, Portfolio owner, string title, HashSet<StockContainer<Share>>? shares,
        HashSet<StockContainer<Bond>>? bonds, HashSet<StockContainer<Currency>>? currencies)
    {
        Id = id;
        Title = title;
        Owner = owner;
        Shares = shares ?? Shares;
        Bonds = bonds ?? Bonds;
        Currencies = currencies ?? Currencies;
    }

    [SetsRequiredMembers]
    public BrokerageAccount(Portfolio owner, string title) : this()
    {
        Owner = owner;
        Title = title;
        Shares = [];
        Bonds = [];
        Currencies = [];
    }

    public Guid Id { get; init; } = Guid.NewGuid();
    public string Title { get; init; } = Guid.NewGuid().ToString();

    [JsonIgnore] public Portfolio Owner { get; init; }
    public Guid OwnerId { get; init; }
    public required HashSet<StockContainer<Share>> Shares { get; init; } = [];
    public required HashSet<StockContainer<Bond>> Bonds { get; init; } = [];
    public required HashSet<StockContainer<Currency>> Currencies { get; init; } = [];

    public Dictionary<string, decimal> GetPricesByCurrenciesTickers(bool withShares = true, bool withBonds = true,
        bool withCurrencies = true)
    {
        if (!withShares && !withBonds && !withCurrencies)
            throw new ArgumentException("At least one asset type (shares, bonds, or currencies) must be included.");
        
        var prices = new Dictionary<string, decimal>();
        
        if (withShares)
            AggregatePrices(Shares);
        if (withBonds)
            AggregatePrices(Bonds);
        if (withCurrencies)
            AggregatePrices(Currencies);

        return prices;

        void AggregatePrices(IEnumerable<dynamic> assets)
        {
            foreach (var asset in assets)
            {
                var currency = asset.OnePurchaseValue.Currency.Value;
                var sumPrice = asset.SumPurchasePrice;

                if (prices.ContainsKey(currency))
                    prices[currency] += sumPrice;
                else
                    prices[currency] = sumPrice;
            }
        }
    }

    public Dictionary<string, MoneyValue> GetValuesByCurrenciesTickers(bool withShares = true, bool withBonds = true,
        bool withCurrencies = true)
    {
        if (!withShares && !withBonds && !withCurrencies)
            throw new ArgumentException("At least one type of data must be provided");

        return Shares.Where(x => withShares)
            .GroupBy(sc => sc.OnePurchaseValue.Currency.Value, sc => sc.SumPurchaseValue)
            .Concat(Bonds.Where(x => withBonds)
                .GroupBy(sc => sc.OnePurchaseValue.Currency.Value, sc => sc.SumPurchaseValue))
            .Concat(Currencies.Where(x => withCurrencies)
                .GroupBy(sc => sc.OnePurchaseValue.Currency.Value, sc => sc.SumPurchaseValue))
            .ToDictionary(x => x.Key, x => x.Sum());
    }

    public StockContainer<T> AddStock<T>(T stock, ulong quantity, out bool isContainerCreated) where T : Stock
    {
        isContainerCreated = false;
        HashSet<StockContainer<T>> stockCollection = GetStockCollection<T>(stock)!;
        var container = stockCollection!.FirstOrDefault(c => c.Equals(new StockContainer<T>(stock, quantity)));
        if (container != null)
        {
            container.AddStock(stock, quantity);
            return container;
        }
        var newContainer = new StockContainer<T>(stock, quantity);
        stockCollection!.Add(newContainer);
        isContainerCreated = true;
        return newContainer;
    }

    public StockContainer<T>? TryRemoveStock<T>(T stock, ulong quantity, [MaybeNullWhen(false)] out bool isContainerDeleted) where T : Stock
    {
        isContainerDeleted = false;
        HashSet<StockContainer<T>> stockCollection = GetStockCollection<T>(stock)!;
        StockContainer<T>? stockContainer = stockCollection.FirstOrDefault(container => container.LastStock.Equals(stock));
        if (stockContainer == null) return null;

        bool isStockRemoved = stockContainer.TryRemoveStock(quantity, stock.Lot, out bool isZero);

        if (!isStockRemoved || !isZero) return stockContainer;

        stockCollection.Remove(stockContainer);
        isContainerDeleted = true;

        return stockContainer;
    }

    private HashSet<StockContainer<T>>? GetStockCollection<T>(T stock) where T : Stock
    {
        return stock switch
        {
            Currency => Currencies as HashSet<StockContainer<T>>,
            Bond => Bonds as HashSet<StockContainer<T>>,
            Share => Shares as HashSet<StockContainer<T>>,
            _ => throw new ArgumentException("Unknown type of stock")
        };
    }

    public StockContainer<Currency>? TryRemoveCurrencies(CurrencyCode code, ulong quantity, [MaybeNullWhen(false)] out bool isContainerDeleted)
    {
        isContainerDeleted = false;
        var currencyContainer = Currencies.FirstOrDefault(x => x.LastStock.Nominal.Currency.Value.Equals(code.Value));
        if (currencyContainer == null) return null;

        bool isCurrencyRemoved = currencyContainer.TryRemoveStock(quantity, 1, out bool isZero);

        if (!isCurrencyRemoved || !isZero) return currencyContainer;

        Currencies.Remove(currencyContainer);
        isContainerDeleted = true;

        return currencyContainer;
    }

    public bool Equals(BrokerageAccount? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return string.Equals(Title, other.Title, StringComparison.Ordinal) && Owner.Equals(other.Owner);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((BrokerageAccount)obj);
    }

    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        hashCode.Add(Title, StringComparer.InvariantCultureIgnoreCase);
        hashCode.Add(Owner);
        return hashCode.ToHashCode();
    }
}