using AutoMapper;
using MediatR;
using Univali.Api.Entities;
using Univali.Api.Repositories;
using Univali.Api.Features.Commands.CreateCustomer.DeleteCustomer;

namespace Univali.Api.Features.Customers.Commands.DeleteCustomer;

// O primeiro parâmetro é o tipo da mensagem
// O segundo parâmetro é o tipo que se espera receber.
public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, DeleteCustomerDto>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    public DeleteCustomerCommandHandler(ICustomerRepository customerRepository, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    public async Task<DeleteCustomerDto> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        var customerFromDatabase = await _customerRepository.GetCustomerByIdAsync(request.Id);

        if (customerFromDatabase == null) return null!;
    
        var customerEntity = _mapper.Map<Customer>(request);
        _customerRepository.DeleteCustomer(customerEntity.Id);
        await _customerRepository.SaveChangesAsync();
        var customerToReturn = _mapper.Map<DeleteCustomerDto>(customerEntity);
        return customerToReturn;
    }
}