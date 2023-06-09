using Univali.Api.Entities;

namespace Univali.Api.Repositores;
 
public interface ICustomerRepository
{
    IEnumerable<Customer> GetCustomers();   

    Customer? GetCustomerById(int customerId);
}