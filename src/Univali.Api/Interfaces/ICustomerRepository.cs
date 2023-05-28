//https://learn.microsoft.com/pt-br/aspnet/mvc/overview/older-versions/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application

using Univali.Api.Entities;

namespace Univali.Api.Interfaces
{
    public interface ICustomerRepository 
    {
        Task<IEnumerable<Customer>> GetCustomer();
        Task<Customer> GetCustomerByID(int customerId);
        void InsertCustomer(Customer student);
        void DeleteStudent(int customerID);
        void UpdateStudent(Customer customer);
        void Save();
    }
}