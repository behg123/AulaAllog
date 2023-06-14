using Microsoft.EntityFrameworkCore;
using Univali.Api.DbContexts;
using Univali.Api.Entities;

namespace Univali.Api.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly CustomerContext _context;
    public CustomerRepository(CustomerContext customerContext)
    {
        _context = customerContext;
    }

    public async Task<Customer?> GetCustomerByIdAsync(int customerId)
    {
        return await _context.Customers.FirstOrDefaultAsync(customer => customer.Id == customerId);
    }

    public async Task<Customer?> GetCustomerWithAddressesByIdAsync(int customerId){
        return await _context.Customers
            .Include(customer => customer.Addresses)
            .FirstOrDefaultAsync(customer => customer.Id == customerId);

    }

    public async Task<IEnumerable<Customer>> GetCustomersAsync()
    {
        return await _context.Customers.ToListAsync();
    }
    public async Task<IEnumerable<Customer>> GetCustomersWithAddressesAsync()
    {
        return await _context.Customers.Include(c => c.Addresses).ToListAsync();
    }

    public void AddCustomer(Customer customer)
    {
        _context.Customers.Add(customer);
    }

    public void DeleteCustomer(int customerId)
    {
        var customer = _context.Customers.FirstOrDefault(customer => customer.Id == customerId)!;
        if (customer != null) _context.Customers.Remove(customer);

    }
    public async Task<bool> SaveChangesAsync()
    {
        return (await _context.SaveChangesAsync() > 0);
    }

    public async Task<Customer?> GetCustomerWithAddressesAsync(int id)
    {
        return await _context.Customers
            .Include(c => c.Addresses)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Customer?> FindCustomerByCpf(string cpf)
    {
        return await _context.Customers.FirstOrDefaultAsync(c => c.Cpf == cpf);
    }

}