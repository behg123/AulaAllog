using AutoMapper;
using MediatR;
using Univali.Api.Features.Customers.Queries.GetCustomers;
using Univali.Api.Models;
using Univali.Api.Repositories;

namespace Univali.Api.Features.Queries.GetCustomers;

public class GetCustomersQueryHandler : IRequestHandler<GetCustomersQuery, GetCustomersDto>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    public GetCustomersQueryHandler(ICustomerRepository customerRepository, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    public async Task<GetCustomersDto> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
    {
        var customersFromDatabase = await _customerRepository.GetCustomersAsync();
        var customerDtos = _mapper.Map<List<CustomerDto>>(customersFromDatabase);

        return new GetCustomersDto
        {
            Customers = customerDtos
        };
    }

}

