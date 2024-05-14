using Application.Interfaces;
using Core.Entities;
using Microsoft.Extensions.Logging;

namespace Persistence.Repositories;

public class ShareRepository : StockRepository<Share>, IShareRepository
{
    public ShareRepository(InvestmentDbContext context, ILoggerFactory loggerFactory) : 
        base(context, loggerFactory.CreateLogger<ShareRepository>()) { }
}