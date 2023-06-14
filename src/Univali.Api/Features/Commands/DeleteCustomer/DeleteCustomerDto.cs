namespace Univali.Api.Features.Customers.Commands.DeleteCustomer;
using Univali.Api.Features.Commands.CreateCustomer.DeleteCustomer;

public class DeleteCustomerDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
}

