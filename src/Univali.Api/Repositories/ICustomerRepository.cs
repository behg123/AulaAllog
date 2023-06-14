using Univali.Api.Entities;

namespace Univali.Api.Repositories;
 
public interface ICustomerRepository
{
    Task<Customer?> GetCustomerByIdAsync(int customerId);

    Task<IEnumerable<Customer>> GetCustomersAsync();
    Task<IEnumerable<Customer>> GetCustomersWithAddressesAsync();
    Task<Customer?> GetCustomerWithAddressesAsync(int id);

    Task<Customer?> FindCustomerByCpf(string cpf);
    void AddCustomer(Customer customer);
    Task<bool> DeleteCustomerAsync(int customerId);
    Task<bool> SaveChangesAsync();
}