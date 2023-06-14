using MediatR;
using Univali.Api.Features.Customers.Queries.GetCustomers;

namespace Univali.Api.Features.Queries.GetCustomers
{
    public class GetCustomersQuery : IRequest<GetCustomersDto>
    {
    }
}
