namespace Univali.Api.Features.Customers.Commands.CreateCustomer;

public interface ICreateCustomerCommandHandler
{
    Task<CreateCustomerDto> Handle(CreateCustomerCommand request);

}