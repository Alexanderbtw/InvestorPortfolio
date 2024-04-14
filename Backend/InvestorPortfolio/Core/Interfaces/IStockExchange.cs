using Core.Entities;

namespace Core.Interfaces;

public interface IStockExchange
{
    Task<IEnumerable<Bond>> GetBondsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Share>> GetSharesAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Currency>> GetCurrenciesAsync(CancellationToken cancellationToken = default);
}