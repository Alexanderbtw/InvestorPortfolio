using Application.Interfaces;
using Core.Entities;
using Core.Entities.Base;
using Core.Entities.SpecificData;
using Core.Interfaces;

namespace Application.Services;

public class AccountService(IUnitOfWork unitOfWork, IStockExchangeService stockExchange)
{
    public async Task<T> SellStock<T>(Guid userId, Guid accountId, string uid, ulong quantity) where T : Stock
    {
        var account = await unitOfWork.Accounts.GetAsync(
                x => x.Id == accountId, 
                [x => x.Currencies, x => x.Shares, x => x.Bonds],
                asNoTracking: false);
        if (account is null || account.OwnerId != userId) throw new NullReferenceException("Account not found");
        
        var sellStock = Activator.CreateInstance(typeof(T)) switch
        {
            Share => await stockExchange.GetShareByUidAsync(uid) as T,
            Bond => await stockExchange.GetBondByUidAsync(uid) as T,
            Currency => await stockExchange.GetCurrencyByUidAsync(uid) as T,
            _ => throw new InvalidOperationException("Invalid stock type")
        };
        ArgumentNullException.ThrowIfNull(sellStock, "Stock not found");
        
        var container = account.TryRemoveStock(sellStock, quantity, out var isContainerDeleted);
        if (!isContainerDeleted)
        {
            unitOfWork.UpdateStock(container!);
        }
        else
        {
            unitOfWork.RemoveStock(container!);
        }
        
        var currencyContainer = account.AddStock(new Currency
        {
            Nominal = sellStock.Nominal,
            OriginalCurrency = sellStock.Nominal.Currency,
        }, quantity * sellStock.Lot, out var isCurrencyCreated);
        if (!isCurrencyCreated)
        {
            unitOfWork.Currencies.Update(currencyContainer);
        }
        else
        {
            await unitOfWork.Currencies.TryAddAsync(currencyContainer);
        }
        
        await unitOfWork.CompleteAsync();
        return sellStock;
    }
        
    public async Task<T> BuyStock<T>(Guid userId, Guid accountId, string uid, ulong quantity) where T : Stock 
    {
        var account = await unitOfWork.Accounts.GetAsync(
                x => x.Id == accountId, 
                [x => x.Currencies, x => x.Shares, x => x.Bonds],
                asNoTracking: false);
        if (account is null || account.OwnerId != userId) throw new NullReferenceException("Account not found");
        
        T? buyStock = Activator.CreateInstance(typeof(T)) switch
        {
            Share => await stockExchange.GetShareByUidAsync(uid) as T,
            Bond => await stockExchange.GetBondByUidAsync(uid) as T,
            Currency => await stockExchange.GetCurrencyByUidAsync(uid) as T,
            _ => throw new InvalidOperationException("Invalid stock type")
        };
        ArgumentNullException.ThrowIfNull(buyStock, "Stock not found");

        var cost = buyStock.Nominal.ToDecimal() * buyStock.Lot * quantity;
        var currencies = account.GetPricesByCurrenciesTickers(withBonds: false, withShares: false); // TODO: Add currency conversion
        if (!currencies.TryGetValue(buyStock.Nominal.Currency.Value, out var value) || value < cost) // TODO: If currency separated by 2 different containers
            throw new InvalidOperationException("Insufficient funds");

        var currencyContainer = account.TryRemoveCurrencies(new CurrencyCode
        {
            Value = buyStock.Nominal.Currency.Value
        }, quantity * buyStock.Lot, out var isCurrencyRemoved);
        if (isCurrencyRemoved)
        {
            unitOfWork.Currencies.Remove(currencyContainer!);
        }
        else
        {
            unitOfWork.Currencies.Update(currencyContainer!);
        }

        var container = account.AddStock(buyStock, quantity, out var isContainerCreated);
        if (!isContainerCreated)
        {
            unitOfWork.UpdateStock(container);
        }
        else
        {
            await unitOfWork.TryAddAsyncStock(container);
        }

        await unitOfWork.CompleteAsync();
        return buyStock;
    }

    public async Task TopUpAccount(Guid userId, Guid accountId, MoneyValue value)
    {
        var account =
            await unitOfWork.Accounts.GetAsync(x => x.Id == accountId, 
                [x => x.Currencies], 
                asNoTracking: false);
        if (account is null || account.OwnerId != userId) throw new NullReferenceException("Account not found");

        var container = account.AddStock(new Currency
        {
            Nominal = new MoneyValue
            {
                Units = 1,
                Nano = (uint)(value.Nano / value.Units),
                Currency = value.Currency
            },
            OriginalCurrency = value.Currency
        }, value.Units, out var isContainerCreated);

        if (!isContainerCreated)
        {
            unitOfWork.Currencies.Update(container);
        }
        else
        {
            await unitOfWork.Currencies.TryAddAsync(container);
        }

        await unitOfWork.CompleteAsync();
    }
    
    public async Task<BrokerageAccount> GetAccountByIdAsync(Guid userId, Guid accountId)
    {
        var account =
            await unitOfWork.Accounts.GetAsync(x => x.Id == accountId, 
                [x => x.Currencies, x => x.Shares, x => x.Bonds], 
                asNoTracking: false);
        if (account is null || account.OwnerId != userId) throw new NullReferenceException("Account not found");
        
        ArgumentNullException.ThrowIfNull(account, "Account not found");
        return account;
    }
}