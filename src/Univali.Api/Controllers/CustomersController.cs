using Microsoft.AspNetCore.Mvc;
using Unvali.Api.Entities;

namespace Univali.Api.Controllers;

[ApiController]
[Route("Api/customers")]
public class CustomersController : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<Customer>> GetCustomers()
    {
        var result = Data.instanceAcess().Customers;
        return Ok(result);
    }

    [HttpGet("id/{id}", Name = "GetCustomerById")]
    public ActionResult<Customer> GetCustomerById([FromRoute] int id)
    {
        var result = Data.instanceAcess().Customers.FirstOrDefault(c => c.Id == id);


        return result != null ? Ok(result) : NotFound();
    }    
    
    [HttpGet("cpf/{cpf}")]
    public ActionResult<Customer> GetCustomerByCpdf([FromRoute] string cpf)
    {
        var result = Data.instanceAcess().Customers.FirstOrDefault(c => c.Cpf == cpf);


        return result != null ? Ok(result) : NotFound();
    }
    [HttpPost]
    public ActionResult<Customer> CreateCustomer(Customer customer)
    {
        var newCustomer = new Customer
        {
            Id = Data.instanceAcess().Customers.Max(c => c.Id) + 1,
            Name = customer.Name,
            Cpf = customer.Cpf

        };

        Data.instanceAcess().Customers.Add(newCustomer);
        return CreatedAtRoute
        (
            "GetCustomerById",
            new {id = newCustomer.Id},
            newCustomer
        );
    }
    
}

