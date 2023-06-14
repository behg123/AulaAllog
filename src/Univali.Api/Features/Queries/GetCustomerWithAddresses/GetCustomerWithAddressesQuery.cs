using MediatR;
using Univali.Api.Features.Customers.Queries.GetCustomers;
using Univali.Api.Features.Queries.GetCustomerWithAddresses;
using Univali.Api.Models;

namespace Univali.Api.Features.Customers.Queries.GetCustomerWithAddresses;

public class GetCustomerWithAddressesQuery : IRequest<GetCustomerWithAddressesDto>
{
    public int Id { get; set; }
}

