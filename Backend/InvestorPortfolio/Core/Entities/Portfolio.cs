using System.Diagnostics.CodeAnalysis;
using Core.Entities.Auth;
using Core.Entities.SpecificData;

namespace Core.Entities;

public class Portfolio
{
    public required User Owner { get; init; }
    public required ISet<BrokerageAccount> Accounts { get; init; }
    public required Guid Id { get; init; }

    public Dictionary<string, decimal> GetPricesByCurrenciesTickers(bool withShares = true, bool withBonds = true, bool withCurrencies = true) =>
        Accounts.Aggregate(new Dictionary<string, decimal>(), (dict, account) =>
        {
            var values = account.GetPricesByCurrenciesTickers();
            foreach (var (key, value) in values)
            {
                dict.TryAdd(key, 0);
                dict[key] += value;
            }

            return dict;
        });

    public bool TryAddAccount(string title, [MaybeNullWhen(false)] out BrokerageAccount? account)
    {
        account = new BrokerageAccount(this, title);
        if (Accounts.All(t => t.Title != title) && Accounts.Add(account))
        {
            return true;
        }
        account = null;
        return false;
    } 
    
    public bool TryAddAccount(BrokerageAccount brokAccount, [MaybeNullWhen(false)] out BrokerageAccount? account)
    {
        if (Accounts.Add(brokAccount))
        {
            account = brokAccount;
            return true;
        }
        account = null;
        return false;
    } 
    
    public bool TryAddAccount(string title, Func<BrokerageAccountBuilder, BrokerageAccount>? builderConfiguration, out BrokerageAccount? account)
    {
        var builder = new BrokerageAccountBuilder(this, title);
        account = builderConfiguration?.Invoke(builder);
        return account is not null && Accounts.Add(account);
    } 
    
    public BrokerageAccount? this[string title] => Accounts.FirstOrDefault(t => t.Title == title);

    public bool DeleteAccount(string title, out BrokerageAccount? account)
    {
        account = Accounts.FirstOrDefault(t => t.Title == title);
        return account is not null && Accounts.Remove(account);
    }
}