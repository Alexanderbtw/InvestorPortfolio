using System.Linq.Expressions;
using Core.Entities;

namespace Application.Interfaces;

public interface IAccountRepository : IGenericRepository<BrokerageAccount>
{
    Task<BrokerageAccount?> GetAccountByIdAsync(Guid id, Expression<Func<BrokerageAccount,object>>[]? includePaths = null, bool asNoTracking = true);
}