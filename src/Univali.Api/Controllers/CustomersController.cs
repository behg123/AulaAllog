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
        var customerDtos = result.Select(customer => ConvertToCustomerDto(customer)).ToList();

        return Ok(customerDtos);
    }


    [HttpGet("{id}", Name = "GetCustomerById")]
    public ActionResult<CustomerDto> GetCustomerById(int id)
    {
        var result = FindCustomerById(id);
        if(result == null){
            return NotFound();
        }

        var customerDto = ConvertToCustomerDto(result);

        return Ok(result);
    }

    [HttpGet("cpf/{cpf}")]
    public ActionResult<CustomerDto> GetCustomerByCpf(string cpf)
    {
        var result = FindCustomerByCpf(cpf);
        var customerDto = ConvertToCustomerDto(result);

        return customerDto != null ? Ok(result) : NotFound();
    }

    [HttpPost]
    public ActionResult<CustomerDto> CreateCustomer(CustomerForCreateDto customer)
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

    [HttpDelete("{id}")]
    public ActionResult<CustomerDto> DeleteCustomer(int id)
    {
        var customer = FindCustomerById(id);

        if (customer == null)
        {
            return NotFound();
        }

        Data.instanceAcess().Customers.Remove(customer);

        return NoContent();

    }


    [HttpPut("{id}")]
    public ActionResult<CustomerDto> UpdateCustomer(int id, CustomerForUpdateDto updatedCustomer)
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
        return Data.instanceAcess().Customers.FirstOrDefault(c => c.Id == id)!;
    }

    private Customer FindCustomerByCpf(String cpf)
    {
        return Data.instanceAcess().Customers.FirstOrDefault(c => c.Cpf == cpf)!;
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

