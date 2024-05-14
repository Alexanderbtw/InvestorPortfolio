using System.Linq.Expressions;
using Application.Interfaces;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Persistence.Repositories;

public class AccountRepository : GenericRepository<BrokerageAccount>, IAccountRepository
{
    public AccountRepository(InvestmentDbContext context, ILoggerFactory loggerFactory) : 
        base(context, loggerFactory.CreateLogger<AccountRepository>()) { }


    public async Task<BrokerageAccount?> GetAccountByIdAsync(Guid id, Expression<Func<BrokerageAccount, object>>[]? includePaths = null, bool asNoTracking = true)
    {
        var res = DbSet.AsQueryable();
        if (asNoTracking)
        {
            res = DbSet.AsNoTracking();
        }

        if (includePaths is not null)
        {
            res = includePaths.Aggregate(res, (current, path) => current.Include(path));
        }
        
        return await res.FirstOrDefaultAsync(u => u.Id == id);
    }
}