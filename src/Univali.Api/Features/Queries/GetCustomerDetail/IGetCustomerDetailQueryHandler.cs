namespace Univali.Api.Features.Customers.Queries.GetCustomerDetail;

public interface IGetCustomerDetailQueryHandler
{
    Task<GetCustomerDetailDto?> Handle(GetCustomerDetailQuery request);
}