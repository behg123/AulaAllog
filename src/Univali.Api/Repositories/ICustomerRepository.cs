using Univali.Api.Entities;

namespace Univali.Api.Repositores;
 
public interface ICustomerRepository
{
    Task<IEnumerable<Customer>> GetCustomersAsync();   
    Task<Customer?> GetCustomerByIdAsync(int customerId);
    void AddCustomer(Customer customer);

    Task<bool> DeleteCustomerAsync(int customerId);

    Task<bool> SaveChangesAsync();

}