using Core.Entities;
using Core.Entities.Auth;
using Core.Entities.Base;

namespace Application.Interfaces;

public interface IUnitOfWork
{
    IUserRepository Users { get; }
    IPortfolioRepository Portfolios { get; }
    IAccountRepository Accounts { get; }
    ICurrencyRepository Currencies { get; set; }
    IShareRepository Shares { get; set; }
    IBondRepository Bonds { get; set; }
    Task<bool> TryAddUserWithPortfolioAsync(User user);
    Task<bool> CompleteAsync();
    void UpdateStock<T>(StockContainer<T> container) where T : Stock;
    Task<bool> TryAddAsyncStock<T>(StockContainer<T> container) where T : Stock;
    void RemoveStock<T>(StockContainer<T> container) where T : Stock;
}