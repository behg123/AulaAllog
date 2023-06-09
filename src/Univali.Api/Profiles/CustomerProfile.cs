using AutoMapper;

namespace Univali.Api.Profiles;

public class CustomerProfile : Profile
{
    public CustomerProfile()
    {
        /*
          1˚ arg tipo do objeto de origem
          2˚ arg tipo do objeto de destino
          Mapeia através dos nomes das propriedades
          Se a propriedade não existir é ignorada
        */
        CreateMap<Entities.Customer, Models.CustomerDto>();
        CreateMap<Entities.Address, Models.AddressDto>(); 
        CreateMap<Entities.Customer, Models.CustomerWithAddressesDto>();
        CreateMap<Entities.Customer, Models.CustomerForPatchDto>();
        CreateMap<Entities.Address, Models.AddressDto>();

        CreateMap<Models.AddressForCreationDto, Entities.Address>();
        CreateMap<Models.AddressDto, Entities.Address>(); 
        CreateMap<Models.CustomerWithAddressesDto, Entities.Customer>();
        CreateMap<Models.CustomerWithAddressesForUpdateDto, Entities.Customer>();
        CreateMap<Models.CustomerForCreationDto, Entities.Customer>();
        CreateMap<Models.CustomerForUpdateDto, Entities.Customer>();
        CreateMap<Models.CustomerWithAddressesForUpdateDto, Entities.Customer>();
        CreateMap<Models.CustomerForPatchDto, Entities.Customer>();
        CreateMap<Models.AddressForUpdateDto, Entities.Address>();
    }
}