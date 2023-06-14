using AutoMapper;
using Univali.Api.Features.Customers.Commands.CreateCustomer;
using Univali.Api.Entities;
using Univali.Api.Models;
using Univali.Api.Features.Commands.CreateCustomer.WithAddresses;
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
        CreateMap<Customer, CustomerDto>();
        CreateMap<Address, AddressDto>(); 
        CreateMap<Customer, CustomerWithAddressesDto>();
        CreateMap<Customer, CustomerForPatchDto>();
        CreateMap<Address, AddressDto>();

        CreateMap<AddressForCreationDto, Address>();
        CreateMap<AddressDto, Address>(); 
        CreateMap<CustomerWithAddressesDto, Customer>();
        CreateMap<CustomerWithAddressesForUpdateDto, Customer>();
        CreateMap<CustomerForCreationDto, Customer>();
        CreateMap<CustomerForUpdateDto, Customer>();
        CreateMap<CustomerWithAddressesForUpdateDto, Customer>();
        CreateMap<CustomerForPatchDto, Customer>();
        CreateMap<AddressForUpdateDto, Address>();


        CreateMap<CreateCustomerCommand, Customer>();
        CreateMap<Customer, CreateCustomerDto>();
        CreateMap<CreateCustomerWithAddressesCommand, Customer>();
        CreateMap<Customer, CreateCustomerWithAddressesDto>();
        CreateMap<Customer, GetCustomerDetailDto>();
        CreateMap<GetCustomerDetailDto, Customer>();

    }
}