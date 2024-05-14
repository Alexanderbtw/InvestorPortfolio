using Application.Interfaces;
using Core.Entities;
using Core.Entities.Base;
using Microsoft.Extensions.Logging;

namespace Persistence.Repositories;

public abstract class StockRepository<T>(InvestmentDbContext context, ILogger logger) : 
    GenericRepository<StockContainer<T>>(context, logger), IStockRepository<T>
    where T : Stock
{
    public void Update(StockContainer<T> stock)
    {
        DbSet.Update(stock);
    }
    
    public void UpdateRange(IEnumerable<StockContainer<T>> stocks)
    {
        DbSet.UpdateRange(stocks);
    }
}