using AutoMapper;
using MediatR;
using Univali.Api.Entities;
using Univali.Api.Repositories;

namespace Univali.Api.Features.Customers.Commands.CreateCustomer;

// O primeiro parâmetro é o tipo da mensagem
// O segundo parâmetro é o tipo que se espera receber.
public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, CreateCustomerDto>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    public CreateCustomerCommandHandler(ICustomerRepository customerRepository, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    public async Task<CreateCustomerDto> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customerEntity = _mapper.Map<Customer>(request);
        _customerRepository.AddCustomer(customerEntity);
        await _customerRepository.SaveChangesAsync();
        var customerToReturn = _mapper.Map<CreateCustomerDto>(customerEntity);
        return customerToReturn;
    }
}