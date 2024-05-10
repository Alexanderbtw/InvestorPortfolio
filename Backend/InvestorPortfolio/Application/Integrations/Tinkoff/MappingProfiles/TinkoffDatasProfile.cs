using AutoMapper;
using Bond = Tinkoff.InvestApi.V1.Bond;
using Currency = Tinkoff.InvestApi.V1.Currency;
using MoneyValue = Tinkoff.InvestApi.V1.MoneyValue;
using Share = Tinkoff.InvestApi.V1.Share;

namespace Application.Integrations.Tinkoff.MappingProfiles;

public class TinkoffDatasProfile : Profile
{
    public TinkoffDatasProfile()
    {
        CreateMap<Bond, Core.Entities.Bond>()
            .ForMember(
                dest => dest.Nominal,
                opt => opt.MapFrom(src => src.PlacementPrice)
            );
        
        CreateMap<Currency, Core.Entities.Currency>()
            .ReverseMap();

        CreateMap<Share, Core.Entities.Share>()
            .ForMember(
                dest => dest.Type,
                opt => opt.MapFrom(src => src.ShareType)
            );
        
        CreateMap<MoneyValue, Core.Entities.SpecificData.MoneyValue>()
            .ForPath(
                dest => dest.Currency.Value,
                opt => opt.MapFrom(src => src.Currency)
            );
    }
}