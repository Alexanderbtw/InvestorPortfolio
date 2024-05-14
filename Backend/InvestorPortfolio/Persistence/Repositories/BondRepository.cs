using Application.Interfaces;
using Core.Entities;
using Microsoft.Extensions.Logging;

namespace Persistence.Repositories;

public class BondRepository : StockRepository<Bond>, IBondRepository
{
    public BondRepository(InvestmentDbContext context, ILoggerFactory loggerFactory) : 
        base(context, loggerFactory.CreateLogger<BondRepository>()) { }
}