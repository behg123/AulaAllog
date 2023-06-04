using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Univali.Api.Entities;
using Univali.Api.Models;


namespace Univali.Api.Controllers;

[ApiController]
[Route("Api/customers")]
public class CustomersController : ControllerBase
{


    ///////////////////////////////////////
    //   ___ _ __ ___  __ _| |_ ___ 
    //  / __| '__/ _ \/ _` | __/ _ \
    // | (__| | |  __/ (_| | ||  __/
    //  \___|_|  \___|\__,_|\__\___|                        
    ///////////////////////////////////////
    [HttpPost]
    public ActionResult<CustomerDto> CreateCustomer(CustomerForCreationDto customerForCreationDto)
    {
        if (!ModelState.IsValid)
        {
            Response.ContentType = "application/problem+json";
            var problemDetailsFactory = HttpContext.RequestServices.GetRequiredService<ProblemDetailsFactory>();
            var validationProblemDetails = problemDetailsFactory.CreateValidationProblemDetails(HttpContext, ModelState);
            validationProblemDetails.Status = StatusCodes.Status422UnprocessableEntity;
            return UnprocessableEntity(validationProblemDetails);
        }
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

    [HttpPost("with-address")]
    public ActionResult<CustomerDto> CreateCustomerWithAddresses(CustomerWithAddressesDto customerWithAddressesDto)
    {
        if (!ModelState.IsValid)
        {
            Response.ContentType = "application/problem+json";
            var problemDetailsFactory = HttpContext.RequestServices.GetRequiredService<ProblemDetailsFactory>();
            var validationProblemDetails = problemDetailsFactory.CreateValidationProblemDetails(HttpContext, ModelState);
            validationProblemDetails.Status = StatusCodes.Status422UnprocessableEntity;
            return UnprocessableEntity(validationProblemDetails);
        }


        var customerEntity = new Customer
        {
            Id = Data.instanceAcess().Customers.Max(c => c.Id) + 1,
            Name = customerWithAddressesDto.Name,
            Cpf = customerWithAddressesDto.Cpf,
        };

        IEnumerable<Address> allAddresses = Data.instanceAcess().Customers.SelectMany(customer => customer.Addresses);

        foreach (var addressDto in customerWithAddressesDto.Addresses)
        {
            var address = new Address
            {
                Id = allAddresses.Max(c => c.Id) + 1,
                Street = addressDto.Street,
                City = addressDto.City
            };

            customerEntity.Addresses.Add(address);
        }


        Data.instanceAcess().Customers.Add(customerEntity);
        var customerToReturn = ConvertToCustomerDto(customerEntity);
        return CreatedAtRoute
        (
            "GetCustomerById",
            new { id = customerToReturn.Id },
            customerToReturn
        );
    }




    ///////////////////////////////////////
    //  _ __ ___  __ _  __| |
    // | '__/ _ \/ _` |/ _` |
    // | | |  __/ (_| | (_| |
    // |_|  \___|\__,_|\__,_|
    ///////////////////////////////////////
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

        [HttpGet("with-address")]
    public ActionResult<IEnumerable<CustomerWithAddressesDto>> GetAllCustomersWithAddresses(int id)
    {
        var customerFromDatabase = Data.instanceAcess().Customers;

        var customerToReturn = customerFromDatabase.Select(customer => new CustomerWithAddressesDto
        {
            Id = customer.Id,
            Name = customer.Name,
            Cpf = customer.Cpf,
            Addresses = customer.Addresses.Select(address => new AddressDto
            {
                Id = address.Id,
                Street = address.Street,
                City = address.City
            }).ToList()
        });

        return Ok(customerToReturn);
    }




    ///////////////////////////////////////
    //  _   _ _ __   __| | __ _| |_ ___ 
    // | | | | '_ \ / _` |/ _` | __/ _ \
    // | |_| | |_) | (_| | (_| | ||  __/
    //  \__,_| .__/ \__,_|\__,_|\__\___|
    //       |_|                        
    ///////////////////////////////////////
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


    [HttpPut("{id}/with-address")]
    public ActionResult<CustomerDto> UpdateCustomerWithAddress(int id, CustomerWithAddressesForUpdateDto customerWithAddressesForUpdateDto)
    {
        if (id != customerWithAddressesForUpdateDto.Id) return BadRequest();
        var customerFromDatabase = FindCustomerById(id);

        if (customerFromDatabase == null) return NotFound();


        customerFromDatabase.Name = customerWithAddressesForUpdateDto.Name;
        customerFromDatabase.Cpf = customerWithAddressesForUpdateDto.Cpf;

        customerFromDatabase.Addresses.Clear();

        IEnumerable<Address> allAddresses = Data.instanceAcess().Customers.SelectMany(customer => customer.Addresses);

        foreach (var addressDto in customerWithAddressesForUpdateDto.Addresses)
        {
            var address = new Address
            {
                Id = allAddresses.Max(c => c.Id) + 1,
                Street = addressDto.Street,
                City = addressDto.City
            };

            customerFromDatabase.Addresses.Add(address);
        }


        var customerToReturn = ConvertToCustomerDto(customerFromDatabase);
        return Ok(customerToReturn);
    }


    [HttpPatch("{id}")]
    public ActionResult PartiallyUpdateCustomer(
        [FromBody] JsonPatchDocument<CustomerForPatchDto> patchDocument,
        [FromRoute] int id)
    {
        var customerFromDatabase = Data.instanceAcess().Customers
            .FirstOrDefault(customer => customer.Id == id);

        if (customerFromDatabase == null) return NotFound();

        var customerToPatch = new CustomerForPatchDto
        {
            Name = customerFromDatabase.Name,
            Cpf = customerFromDatabase.Cpf
        };

        patchDocument.ApplyTo(customerToPatch);

        customerFromDatabase.Name = customerToPatch.Name;
        customerFromDatabase.Cpf = customerToPatch.Cpf;

        return NoContent();

    }
    ///////////////////////////////////////
    //   __| | ___| | ___| |_ ___ 
    //  / _` |/ _ \ |/ _ \ __/ _ \
    // | (_| |  __/ |  __/ ||  __/
    //  \__,_|\___|_|\___|\__\___|
    ///////////////////////////////////////                 
    [HttpDelete("{id}")]
    public ActionResult<CustomerDto> DeleteCustomer(int id)
    {
        var customerFromDatabase = FindCustomerById(id);
        if (customerFromDatabase == null) return NotFound();
        Data.instanceAcess().Customers.Remove(customerFromDatabase);
        return NoContent();
    }


    ///////////////////////////////////////                 
    //  _   _| |_(_) |___ 
    // | | | | __| | / __|
    // | |_| | |_| | \__ \
    //  \__,_|\__|_|_|___/
    ///////////////////////////////////////                 
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

