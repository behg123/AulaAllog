using Univali.Api.Entities;

namespace Univali.Api.Repositories;
 
public interface ICustomerRepository
{
    Task<Customer?> GetCustomerByIdAsync(int customerId);
    Task<Customer?> GetCustomerWithAddressesByIdAsync(int customerId);

    Task<IEnumerable<Customer>> GetCustomersAsync();
    Task<IEnumerable<Customer>> GetCustomersWithAddressesAsync();
    Task<Customer?> GetCustomerWithAddressesAsync(int id);

    Task<Customer?> FindCustomerByCpf(string cpf);
    void AddCustomer(Customer customer);
    void DeleteCustomer(int customerId);
    Task<bool> SaveChangesAsync();
}