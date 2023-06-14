using MediatR;

namespace Univali.Api.Features.Commands.CreateCustomer.DeleteCustomer
{
    public class DeleteCustomerCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }

}
