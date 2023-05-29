using Microsoft.AspNetCore.Mvc;
using Univali.Api.Entities;
using Univali.Api.Dto;


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
        var result = FindCustomerById(id);

        return result != null ? Ok(result) : NotFound();
    }

    [HttpGet("cpf/{cpf}")]
    public ActionResult<Customer> GetCustomerByCpdf([FromRoute] string cpf)
    {
        var result = FindCustomerByCpf(cpf);


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
            new { id = newCustomer.Id },
            newCustomer
        );
    }

    [HttpDelete("delete/{id}")]
    public ActionResult<Customer> DeleteCustomer([FromRoute] int id)
    {
        var customer = FindCustomerById(id);
        if (customer == null)
        {
            return NotFound();
        }

        Data.instanceAcess().Customers.Remove(customer);

        return NoContent();

    }



    [HttpPut("update/{id}")]
    public ActionResult<Customer> UpdateCustoner([FromRoute] int id, [FromBody] CustomerDto updatedCustomer)
    {
        var customer = FindCustomerById(id);
        if (customer == null)
        {
            return NotFound();
        }
        ConvertToCustomer(id, updatedCustomer);

        return Ok(customer);


    }

    private Customer FindCustomerById(int id)
    {
        return Data.instanceAcess().Customers.FirstOrDefault(c => c.Id == id);
    }

    private Customer FindCustomerByCpf(String cpf)
    {
        return Data.instanceAcess().Customers.FirstOrDefault(c => c.Cpf == cpf);
    }
    private Customer ConvertToCustomer(int customerId, CustomerDto customerDto)
    {
        var customer = new Customer
        {
            Id = customerId,
            Name = customerDto.Name,
            Cpf = customerDto.Cpf
        };

        return customer;
    }
}

