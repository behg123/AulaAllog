using Univali.Api.Entities;

namespace Univali.Api.Repositores;
 
public interface ICustomerRepository
{
    Task<IEnumerable<Customer>> GetCustomersAsync();   

    Customer? GetCustomerById(int customerId);
    
}