using Univali.Api.DbContexts;
using Univali.Api.Entities;

namespace Univali.Api.Repositores;

public class CustomerRepository : ICustomerRepository
{
    private readonly CustomerContext _context;
    public CustomerRepository(CustomerContext customerContext)
    {
        _context = customerContext;
    }

    public Customer? GetCustomerById(int customerId)
    {
        return _context.Customers.FirstOrDefault(customer => customer.Id == customerId)!;
    }

    public IEnumerable<Customer> GetCustomers()
    {
        return _context.Customers.OrderBy(customer => customer.Name).ToList();
    }
}