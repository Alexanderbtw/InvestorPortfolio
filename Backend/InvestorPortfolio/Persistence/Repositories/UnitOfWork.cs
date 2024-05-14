using Application.Interfaces;
using Core.Entities;
using Core.Entities.Auth;
using Core.Entities.Base;
using Microsoft.Extensions.Logging;

namespace Persistence.Repositories;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly InvestmentDbContext _context;
    
    public UnitOfWork(InvestmentDbContext context, ILoggerFactory loggerFactory)
    {
        _context = context;

        Users = new UserRepository(_context, loggerFactory);
        Portfolios = new PortfolioRepository(_context, loggerFactory);
        Accounts = new AccountRepository(_context, loggerFactory);
        Currencies = new CurrencyRepository(_context, loggerFactory);
        Shares = new ShareRepository(_context, loggerFactory);
        Bonds = new BondRepository(_context, loggerFactory);
    }

    public IUserRepository Users { get; }
    public IPortfolioRepository Portfolios { get; }
    public IAccountRepository Accounts { get; }
    public ICurrencyRepository Currencies { get; set; }
    public IShareRepository Shares { get; set; }
    public IBondRepository Bonds { get; set; }

    public async Task<bool> TryAddUserWithPortfolioAsync(User user)
    {
        var portfolio = new Portfolio
        {
            Id = user.Id,
            Accounts = new HashSet<BrokerageAccount>(),
            Owner = user
        };

        return
            await Portfolios.TryAddAsync(portfolio);
    }

    public async Task<bool> CompleteAsync()
    {
        var res = await _context.SaveChangesAsync();
        return res > 0;
    }

    public void UpdateStock<T>(StockContainer<T> container) where T : Stock
    {
        switch (container)
        {
            case StockContainer<Share> shareContainer:
                Shares.Update(shareContainer);
                break;
            case StockContainer<Bond> bondContainer:
                Bonds.Update(bondContainer);
                break;
            case StockContainer<Currency> currencyContainer:
                Currencies.Update(currencyContainer);
                break;
            default:
                throw new InvalidOperationException("Invalid stock type");
        }
    }

    public async Task<bool> TryAddAsyncStock<T>(StockContainer<T> container) where T : Stock
    {
        return container switch
        {
            StockContainer<Share> shareContainer => await Shares.TryAddAsync(shareContainer),
            StockContainer<Bond> bondContainer => await Bonds.TryAddAsync(bondContainer),
            StockContainer<Currency> currencyContainer => await Currencies.TryAddAsync(currencyContainer),
            _ => throw new InvalidOperationException("Invalid stock type")
        };
    }

    public void RemoveStock<T>(StockContainer<T> container) where T : Stock
    {
        switch (container)
        {
            case StockContainer<Share> shareContainer:
                Shares.Remove(shareContainer);
                break;
            case StockContainer<Bond> bondContainer:
                Bonds.Remove(bondContainer);
                break;
            case StockContainer<Currency> currencyContainer:
                Currencies.Remove(currencyContainer);
                break;
            default:
                throw new InvalidOperationException("Invalid stock type");
        }
    }

    public void Dispose() => _context.Dispose();
}