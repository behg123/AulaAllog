using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Univali.Api.Entities;
using Univali.Api.Models;


namespace Univali.Api.Controllers;

[ApiController]
[Route("Api/customers")]
public class CustomersController : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<CustomerDto>> GetCustomers()
    {
        var customerFromDatabase = Data.instanceAcess().Customers;
        var customerDtos = customerFromDatabase.Select(customer => ConvertToCustomerDto(customer)).ToList();
        return Ok(customerDtos);
    }


    [HttpGet("{id}", Name = "GetCustomerById")]
    public ActionResult<CustomerDto> GetCustomerById(int id)
    {
        var customerFromDatabase = FindCustomerById(id);
        if (customerFromDatabase == null) return NotFound();
        var customerToReturn = ConvertToCustomerDto(customerFromDatabase);
        return Ok(customerToReturn);
    }


    [HttpGet("cpf/{cpf}")]
    public ActionResult<CustomerDto> GetCustomerByCpf(string cpf)
    {
        var customerFromDatabase = FindCustomerByCpf(cpf);
        if (customerFromDatabase == null) return NotFound();
        var customerToReturn = ConvertToCustomerDto(customerFromDatabase);
        return Ok(customerToReturn);
    }


    [HttpPost]
    public ActionResult<CustomerDto> CreateCustomer(CustomerForCreationDto customerForCreationDto)
    {
        var customerEntity = new Customer
        {
            Id = Data.instanceAcess().Customers.Max(c => c.Id) + 1,
            Name = customerForCreationDto.Name,
            Cpf = customerForCreationDto.Cpf
        };
        Data.instanceAcess().Customers.Add(customerEntity);
        var customerToReturn = ConvertToCustomerDto(customerEntity);
        return CreatedAtRoute
        (
            "GetCustomerById",
            new { id = customerToReturn.Id },
            customerToReturn
        );
    }


    [HttpPut("{id}")]
    public ActionResult<CustomerDto> UpdateCustomer(int id, CustomerForUpdateDto customerForUpdateDto)
    {
        if (id != customerForUpdateDto.Id) return BadRequest();
        var customerFromDatabase = FindCustomerById(id);
        if (customerFromDatabase == null) return NotFound();
        customerFromDatabase.Name = customerForUpdateDto.Name;
        customerFromDatabase.Cpf = customerForUpdateDto.Cpf;
        var customerToReturn = ConvertToCustomerDto(customerFromDatabase);
        return Ok(customerToReturn);
    }


    [HttpDelete("{id}")]
    public ActionResult<CustomerDto> DeleteCustomer(int id)
    {
        var customerFromDatabase = FindCustomerById(id);
        if (customerFromDatabase == null) return NotFound();
        Data.instanceAcess().Customers.Remove(customerFromDatabase);
        return NoContent();
    }



    [HttpPatch("id")]
    public ActionResult PartiallyUpdateCustomer([FromBody] JsonPatchDocument<CustomerForPatchDto> patchDocument, [FromRoute] int id)
    {
        var customerFromDatabase = FindCustomerById(id);

        if (customerFromDatabase == null) return NotFound();

        var customerToPatch = new CustomerForPatchDto()
        {
                Name = customerFromDatabase.Name,
                Cpf = customerFromDatabase.Cpf,

        };

        patchDocument.ApplyTo(customerToPatch);

        customerFromDatabase.Name = customerToPatch.Name;
        customerFromDatabase.Cpf = customerToPatch.Cpf;

        return NoContent();
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

