using Application.Interfaces;
using Core.Entities;
using Core.Interfaces;

namespace Application.Services;

public class PortfolioService(IUnitOfWork unitOfWork, IStockExchangeService stockExchange)
{
    public async Task<Portfolio> GetPortfolioByIdAsync(Guid userId)
    {
        var portfolio = await unitOfWork.Portfolios.GetPortfolioByIdAsync(userId, [x => x.Accounts, x => x.Owner]);
        if (portfolio is null) throw new NullReferenceException("Portfolio not found");
        return portfolio; 
    }
    
    public async Task<BrokerageAccount> AddAccountAsync(Guid userId, string accountTitle)
    {
        var portfolio = await unitOfWork.Portfolios.GetPortfolioByIdAsync(userId, [x => x.Accounts], false);
        if (portfolio is null) throw new NullReferenceException("Portfolio not found");

        if (!portfolio.TryAddAccount(accountTitle, out var account))
            throw new InvalidOperationException("Account already exists");
        
        await unitOfWork.Accounts.TryAddAsync(account!);
        await unitOfWork.CompleteAsync();
        return account!;
    }
}