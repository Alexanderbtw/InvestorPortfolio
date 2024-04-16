using AutoMapper;
using Tinkoff.InvestApi.V1;

namespace Application.Integration.Tinkoff.MappingProfiles;

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
                dest => dest.Currency.Code,
                opt => opt.MapFrom(src => src.Currency)
            );
    }
}