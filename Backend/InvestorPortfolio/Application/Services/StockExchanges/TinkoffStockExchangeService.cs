using AutoMapper;
using Core.Interfaces;
using Tinkoff.InvestApi;
using Tinkoff.InvestApi.V1;
using Bond = Core.Entities.Bond;
using Currency = Core.Entities.Currency;
using MoneyValue = Core.Entities.SpecificData.MoneyValue;
using Share = Core.Entities.Share;

namespace Application.Integration.Tinkoff;

public class TinkoffStockExchangeService : IStockExchange
{
    private readonly InvestApiClient _client;
    private readonly IMapper _mapper; 

    public TinkoffStockExchangeService(InvestApiClient client, IMapper mapper)
    {
        _client = client;
        _mapper = mapper;
    }

    public async Task<IEnumerable<Bond>> GetBondsAsync(CancellationToken cancellationToken = default)
    {
        var response = await _client.Instruments.BondsAsync(
            new InstrumentsRequest { InstrumentStatus = InstrumentStatus.Base },
            cancellationToken: cancellationToken);
        return _mapper.Map<IEnumerable<Bond>>(response.Instruments);
    }

    public async Task<IEnumerable<Share>> GetSharesAsync(CancellationToken cancellationToken = default)
    {
        var response = await _client.Instruments.SharesAsync(
            new InstrumentsRequest { InstrumentStatus = InstrumentStatus.Base },
            cancellationToken: cancellationToken);
        return _mapper.Map<IEnumerable<Share>>(response.Instruments);
    }

    public async Task<IEnumerable<Currency>> GetCurrenciesAsync(CancellationToken cancellationToken = default)
    {
        var response = await _client.Instruments.CurrenciesAsync(
            new InstrumentsRequest { InstrumentStatus = InstrumentStatus.Base },
            cancellationToken: cancellationToken);
        return _mapper.Map<IEnumerable<Currency>>(response.Instruments);
    }
}