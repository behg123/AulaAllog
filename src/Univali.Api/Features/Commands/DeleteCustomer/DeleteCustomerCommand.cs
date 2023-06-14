using MediatR;
using Univali.Api.Features.Customers.Commands.DeleteCustomer;

namespace Univali.Api.Features.Commands.CreateCustomer.DeleteCustomer
{
    public class DeleteCustomerCommand : IRequest<DeleteCustomerDto>
    {
        public int Id { get; set; }
    }

}
