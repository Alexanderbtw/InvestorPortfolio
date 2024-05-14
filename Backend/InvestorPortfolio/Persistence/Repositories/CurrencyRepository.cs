using Application.Interfaces;
using Core.Entities;
using Microsoft.Extensions.Logging;

namespace Persistence.Repositories;

public class CurrencyRepository : StockRepository<Currency>, ICurrencyRepository
{
    public CurrencyRepository(InvestmentDbContext context, ILoggerFactory loggerFactory) : 
        base(context, loggerFactory.CreateLogger<CurrencyRepository>()) { }
}