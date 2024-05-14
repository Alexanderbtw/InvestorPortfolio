using AutoMapper;
using Core.Interfaces;
using Tinkoff.InvestApi;
using Tinkoff.InvestApi.V1;
using Bond = Core.Entities.Bond;
using Currency = Core.Entities.Currency;
using Share = Core.Entities.Share;

namespace Application.Services.StockExchanges;

public class TinkoffStockExchangeServiceService(InvestApiClient client, IMapper mapper) : IStockExchangeService
{
    public async Task<IEnumerable<Bond>> GetBondsAsync(CancellationToken cancellationToken = default)
    {
        var response = await client.Instruments.BondsAsync(
            new InstrumentsRequest { InstrumentStatus = InstrumentStatus.Base },
            cancellationToken: cancellationToken);
        return mapper.Map<IEnumerable<Bond>>(response.Instruments);
    }

    public async Task<IEnumerable<Share>> GetSharesAsync(CancellationToken cancellationToken = default)
    {
        var response = await client.Instruments.SharesAsync(
            new InstrumentsRequest { InstrumentStatus = InstrumentStatus.Base },
            cancellationToken: cancellationToken);
        return mapper.Map<IEnumerable<Share>>(response.Instruments);
    }

    public async Task<IEnumerable<Currency>> GetCurrenciesAsync(CancellationToken cancellationToken = default)
    {
        var response = await client.Instruments.CurrenciesAsync(
            new InstrumentsRequest { InstrumentStatus = InstrumentStatus.Base },
            cancellationToken: cancellationToken);
        return mapper.Map<IEnumerable<Currency>>(response.Instruments);
    }

    public async Task<Share?> GetShareByUidAsync(string uid, CancellationToken cancellationToken = default)
    {
        var response = await client.Instruments.ShareByAsync(
            new InstrumentRequest { Id = uid, IdType = InstrumentIdType.Uid },
            cancellationToken: cancellationToken);

        return mapper.Map<Share>(response.Instrument);
    }

    public async Task<Bond?> GetBondByUidAsync(string uid, CancellationToken cancellationToken = default)
    {
        var response = await client.Instruments.BondByAsync(
            new InstrumentRequest { Id = uid, IdType = InstrumentIdType.Uid },
            cancellationToken: cancellationToken);

        return mapper.Map<Bond>(response.Instrument);
    }

    public async Task<Currency?> GetCurrencyByUidAsync(string uid, CancellationToken cancellationToken = default)
    {
        var response = await client.Instruments.CurrencyByAsync(
            new InstrumentRequest { Id = uid, IdType = InstrumentIdType.Uid },
            cancellationToken: cancellationToken);

        return mapper.Map<Currency>(response.Instrument);
    }
}