using Univali.Api.Models;

namespace Univali.Api.Features.Queries.GetCustomers
{
    public interface IGetCustomersQueryHandler
    {
        Task<IEnumerable<CustomerDto>> Handle(GetCustomersQuery request);
    }
}
