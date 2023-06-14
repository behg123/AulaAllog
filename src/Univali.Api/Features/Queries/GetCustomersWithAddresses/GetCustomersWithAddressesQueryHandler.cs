using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Univali.Api.Features.Queries.GetCustomersWithAddresses;
using Univali.Api.Models;
using Univali.Api.Repositories;

namespace Univali.Api.Features.QueryHandlers.GetCustomersWithAddresses
{
    public class GetCustomersWithAddressesQueryHandler : IRequestHandler<GetCustomersWithAddressesQuery, GetCustomerWithAddressesDto>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public GetCustomersWithAddressesQueryHandler(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public async Task<GetCustomerWithAddressesDto> Handle(GetCustomersWithAddressesQuery request, CancellationToken cancellationToken)
        {
            var customersFromDatabase = await _customerRepository.GetCustomersWithAddressesAsync();
            var customerDtos = _mapper.Map<List<CustomerWithAddressesDto>>(customersFromDatabase);

            return new GetCustomerWithAddressesDto
            {
                Customers = customerDtos
            };
        }
    }
}
