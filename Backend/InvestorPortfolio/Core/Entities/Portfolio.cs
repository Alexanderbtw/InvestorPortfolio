﻿using System.Diagnostics.CodeAnalysis;

namespace Core.Entities;

public class Portfolio
{
    public Guid Id { get; init; }
    private readonly Dictionary<string, BrokerageAccount> _accounts = new();
    
    public bool TryAddAccount(string title, [MaybeNullWhen(false)] out BrokerageAccount? account)
    {
        account = new BrokerageAccount(this, title);
        if (_accounts.TryAdd(title, account)) return true;
        account = null;
        return false;
    } 
    public bool TryAddAccount(string title, Func<BrokerageAccountBuilder, BrokerageAccount>? builderConfiguration, out BrokerageAccount? account)
    {
        var builder = new BrokerageAccountBuilder(this, title);
        account = builderConfiguration?.Invoke(builder);
        return account is not null && _accounts.TryAdd(account.Title, account);
    } 
    
    public BrokerageAccount? this[string title] => _accounts[title];
    
    public IEnumerable<BrokerageAccount> Accounts => _accounts.Values;

    public bool DeleteAccount(string title, out BrokerageAccount? account)
    {
        return _accounts.Remove(title, out account);
    }
}