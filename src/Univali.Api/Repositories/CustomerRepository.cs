//https://learn.microsoft.com/pt-br/aspnet/mvc/overview/older-versions/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Univali.Api.Entities;
using Univali.Api.Interfaces;
using Univali.Api.Models;

namespace Univali.Api.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly CustomerContext _context;

    public CustomerRepository(CustomerContext context)
    {
        _context = context;
    }

    public Task<IEnumerable<Customer>> GetCustomer()
    {
        return _context.Customers.ToList();
    }
    public Task<Customer> GetCustomerByID(int customerId)
    {
        throw new NotImplementedException();
    }
    public void InsertCustomer(Customer student)
    {
        throw new NotImplementedException();
    }
    public void DeleteStudent(int customerID)
    {
        throw new NotImplementedException();
    }
    public void UpdateStudent(Customer customer)
    {
        throw new NotImplementedException();
    }
    public void Save()
    {
        throw new NotImplementedException();
    }
}
