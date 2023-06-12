using AutoMapper;
using Univali.Api.Entities;
using Univali.Api.Features.Customers.Commands.CreateCustomer;
using Univali.Api.Features.Customers.Queries.GetCustomerDetail;

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

        CreateMap<Entities.Customer, Models.CustomerForPatchDto>();

        CreateMap<Models.CustomerWithAddressesDto, Entities.Customer>();
        CreateMap<Models.CustomerWithAddressesForUpdateDto, Entities.Customer>();
        CreateMap<Models.CustomerForCreationDto, Entities.Customer>();
        CreateMap<Models.CustomerForUpdateDto, Entities.Customer>();
        CreateMap<Models.CustomerWithAddressesForUpdateDto, Entities.Customer>();
        CreateMap<Models.CustomerForPatchDto, Entities.Customer>();

        //Novos
        CreateMap<Customer, GetCustomerDetailDto>();
        CreateMap<CreateCustomerCommand, Customer>();
        CreateMap<GetCustomerDetailDto, Customer>();
        CreateMap<Customer, CreateCustomerCommand>();
    }
}