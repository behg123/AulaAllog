using Microsoft.AspNetCore.Mvc;
using Univali.Api.Entities;
using Univali.Api.Dto;


namespace Univali.Api.Controllers;

[ApiController]
[Route("Api/customers")]
public class CustomersController : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<CustomerDto>> GetCustomers()
    {
        var result = Data.instanceAcess().Customers;
        var customerDtos = result.Select(customer => ConvertToCustomerDto(customer));


        return Ok(customerDtos);
    }


    [HttpGet("id/{id}", Name = "GetCustomerById")]
    public ActionResult<CustomerDto> GetCustomerById([FromRoute] int id)
    {
        var result = FindCustomerById(id);
        var customerDto = ConvertToCustomerDto(result);

        return customerDto != null ? Ok(result) : NotFound();
    }

    [HttpGet("cpf/{cpf}")]
    public ActionResult<CustomerDto> GetCustomerByCpdf([FromRoute] string cpf)
    {
        var result = FindCustomerByCpf(cpf);
        var customerDto = ConvertToCustomerDto(result);

        return customerDto != null ? Ok(result) : NotFound();
    }

    [HttpPost]
    public ActionResult<CustomerDto> CreateCustomer([FromBody] CustomerDto customer)
    {
        var newCustomer = new Customer
        {
            Id = Data.instanceAcess().Customers.Max(c => c.Id) + 1,
            Name = customer.Name,
            Cpf = customer.Cpf

        };

        var newCustomerDto = ConvertToCustomerDto(newCustomer);

        Data.instanceAcess().Customers.Add(newCustomer);

        return CreatedAtRoute
        (
            "GetCustomerById",
            new { id = newCustomerDto.Id },
            newCustomerDto
        );
    }

    [HttpDelete("delete/{id}")]
    public ActionResult<CustomerDto> DeleteCustomer([FromRoute] int id)
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
    public ActionResult<CustomerDto> UpdateCustomer([FromRoute] int id, [FromBody] CustomerDto updatedCustomer)
    {
        var customer = FindCustomerById(id);
        if (customer == null)
        {
            return NotFound();
        }

        customer.Name = updatedCustomer.Name;
        customer.Cpf = updatedCustomer.Cpf;

        var updatedCustomerDto = ConvertToCustomerDto(customer);

        return Ok(updatedCustomerDto);
    }

    private Customer FindCustomerById(int id)
    {
        return Data.instanceAcess().Customers.FirstOrDefault(c => c.Id == id);
    }

    private Customer FindCustomerByCpf(String cpf)
    {
        return Data.instanceAcess().Customers.FirstOrDefault(c => c.Cpf == cpf);
    }

    private CustomerDto ConvertToCustomerDto(Customer customer)
    {
        var customerDto = new CustomerDto
        {
            Id = customer.Id,
            Name = customer.Name,
            Cpf = customer.Cpf
        };

        return customerDto;
    }

    private Customer ConvertToCustomer(CustomerDto customerDto)
    {
        var newCustomer = new Customer
        {
            Id = customerDto.Id,
            Name = customerDto.Name,
            Cpf = customerDto.Cpf
        };

        return newCustomer;
    }

}

