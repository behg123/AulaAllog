using Univali.Api.Entities;
using Univali.Api.Features.Customers.Commands.CreateCustomer;
using AutoMapper;
using Univali.Api.Repositories;
using MediatR;

namespace Univali.Api.Features.Commands.CreateCustomer.WithAddresses
{

    public class CreateCustomerWithAddressesCommandHandler : IRequestHandler<CreateCustomerWithAddressesCommand, CreateCustomerWithAddressesDto>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public CreateCustomerWithAddressesCommandHandler(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }
        public async Task<CreateCustomerWithAddressesDto> Handle(CreateCustomerWithAddressesCommand request, CancellationToken cancellationToken)
        {
            var customerEntity = _mapper.Map<Customer>(request);
            _customerRepository.AddCustomer(customerEntity);
            await _customerRepository.SaveChangesAsync();
            var customerToReturn = _mapper.Map<CreateCustomerWithAddressesDto>(customerEntity);
            return customerToReturn;
        }
    }
}
