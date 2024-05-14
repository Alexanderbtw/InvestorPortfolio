using Core.Entities;

namespace Core.Interfaces;

public interface IStockExchangeService
{
    Task<IEnumerable<Bond>> GetBondsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Share>> GetSharesAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Currency>> GetCurrenciesAsync(CancellationToken cancellationToken = default);
    Task<Share?> GetShareByUidAsync(string isin, CancellationToken cancellationToken = default);
    Task<Bond?> GetBondByUidAsync(string isin, CancellationToken cancellationToken = default);
    Task<Currency?> GetCurrencyByUidAsync(string isin, CancellationToken cancellationToken = default);
}