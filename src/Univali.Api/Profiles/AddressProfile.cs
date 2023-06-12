using AutoMapper;
using Univali.Api.Entities;
using Univali.Api.Features.Customers.Commands.CreateCustomer;
using Univali.Api.Features.Customers.Queries.GetCustomerDetail;

namespace Univali.Api.Profiles;

public class AddressProfile : Profile
{
    public AddressProfile()
    {
        /*
          1˚ arg tipo do objeto de origem
          2˚ arg tipo do objeto de destino
          Mapeia através dos nomes das propriedades
          Se a propriedade não existir é ignorada
        */
        CreateMap<Models.AddressForUpdateDto, Entities.Address>();
        CreateMap<Entities.Address, Models.AddressDto>();
        CreateMap<Models.AddressForCreationDto, Entities.Address>();
        CreateMap<Models.AddressDto, Entities.Address>(); 
        CreateMap<Entities.Address, Models.AddressDto>(); 
        CreateMap<Entities.Customer, Models.CustomerWithAddressesDto>();
    }
}