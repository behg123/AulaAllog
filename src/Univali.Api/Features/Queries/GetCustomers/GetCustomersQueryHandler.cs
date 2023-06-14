using AutoMapper;
using Univali.Api.Models;
using Univali.Api.Repositories;

namespace Univali.Api.Features.Queries.GetCustomers;

public class GetCustomersQueryHandler : IGetCustomersQueryHandler
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    public GetCustomersQueryHandler(ICustomerRepository customerRepository, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CustomerDto>> Handle(GetCustomersQuery request)
    {
        var customersFromDatabase = await _customerRepository.GetCustomersAsync();
        var customerDtos = _mapper.Map<IEnumerable<CustomerDto>>(customersFromDatabase);
        return customerDtos;
    }
}

