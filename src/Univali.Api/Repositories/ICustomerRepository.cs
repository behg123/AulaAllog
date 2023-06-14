using Univali.Api.Entities;

namespace Univali.Api.Repositories;
 
public interface ICustomerRepository
{
    Task<Customer?> GetCustomerByIdAsync(int customerId);

    Task<IEnumerable<Customer>> GetCustomersAsync();
    void AddCustomer(Customer customer);
    Task<bool> DeleteCustomerAsync(int customerId);
    Task<bool> SaveChangesAsync();

    Task<bool> UpdateCustomerAsync(Customer customer);
}