using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Univali.Api.Features.Customers.Queries.GetCustomerWithAddresses;
using Univali.Api.Features.Customers.Queries.GetCustomers;
using Univali.Api.Models;
using Univali.Api.Repositories;
using Univali.Api.Features.Queries.GetCustomerWithAddresses;

namespace Univali.Api.Features.Handlers.Customers
{
    public class GetCustomerWithAddressesQueryHandler : IRequestHandler<GetCustomerWithAddressesQuery, GetCustomerWithAddressesDto>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public GetCustomerWithAddressesQueryHandler(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public async Task<GetCustomerWithAddressesDto> Handle(GetCustomerWithAddressesQuery request, CancellationToken cancellationToken)
        {
            var customerFromDatabase = await _customerRepository.GetCustomerWithAddressesAsync(request.Id);

            if (customerFromDatabase == null)
            {
                return null!;
            }

            var customerWithAddressesDto = _mapper.Map<GetCustomerWithAddressesDto>(customerFromDatabase);
            return customerWithAddressesDto;
        }
    }
}
