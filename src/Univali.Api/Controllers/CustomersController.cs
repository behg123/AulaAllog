using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using Univali.Api.DbContexts;
using Univali.Api.Entities;
using Univali.Api.Models;

namespace Univali.Api.Controllers;

[ApiController]
[Route("Api/customers")]
public class CustomersController : MainController
{

    private readonly Data _data;
    private readonly IMapper _mapper;
    private readonly CustomerContext _context;


    public CustomersController(Data data, IMapper mapper, CustomerContext context)
    {
        _data = data ?? throw new ArgumentNullException(nameof(data));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    ///////////////////////////////////////
    //   ___ _ __ ___  __ _| |_ ___ 
    //  / __| '__/ _ \/ _` | __/ _ \
    // | (__| | |  __/ (_| | ||  __/
    //  \___|_|  \___|\__,_|\__\___|                        
    ///////////////////////////////////////
    [HttpPost]
    public ActionResult<CustomerDto> CreateCustomer(CustomerForCreationDto customerForCreationDto)
    {
        // if (!ModelState.IsValid)
        // {
        //     Response.ContentType = "application/problem+json";
        //     var problemDetailsFactory = HttpContext.RequestServices.GetRequiredService<ProblemDetailsFactory>();
        //     var validationProblemDetails = problemDetailsFactory.CreateValidationProblemDetails(HttpContext, ModelState);
        //     validationProblemDetails.Status = StatusCodes.Status422UnprocessableEntity;
        //     return UnprocessableEntity(validationProblemDetails);
        // }

        var customerEntity = _mapper.Map<Customer>(customerForCreationDto);
        _context.Customers.Add(customerEntity);
        _context.SaveChanges();

        var customerToReturn = _mapper.Map<CustomerDto>(customerEntity);

        return CreatedAtRoute
        (
            "GetCustomerById",
            new { id = customerToReturn.Id },
            customerToReturn
        );
    }

    [HttpPost("with-addresses")]
    public ActionResult<CustomerDto> CreateCustomerWithAddresses(CustomerWithAddressesDto customerWithAddressesDto)
    {
        var customerEntity = new Customer
        {
            Id = _data.Customers.Max(c => c.Id) + 1,
            Name = customerWithAddressesDto.Name,
            Cpf = customerWithAddressesDto.Cpf,
        };

        IEnumerable<Address> allAddresses = _data.Customers.SelectMany(customer => customer.Addresses);

        int iterator = 1;
        foreach (var addressDto in customerWithAddressesDto.Addresses)
        {
            var address = new Address
            {
                Id = allAddresses.Max(c => c.Id) + iterator,
                Street = addressDto.Street,
                City = addressDto.City
            };
            customerEntity.Addresses.Add(address);
            iterator = iterator + 1;
        }

        _data.Customers.Add(customerEntity);
        var customerToReturn = _mapper.Map<CustomerDto>(customerEntity);
        return CreatedAtRoute
        (
            "GetCustomerWithAddressesById",
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
        var customerFromDatabase = _context.Customers.OrderBy(c => c.Name).ToList();
        var customerDtos = _mapper.Map<IEnumerable<CustomerDto>>(customerFromDatabase);
        return Ok(customerDtos);
    }

    [HttpGet("{id}", Name = "GetCustomerById")]
    public ActionResult<CustomerDto> GetCustomerById(int id)
    {
        var customerFromDatabase = FindCustomerById(id);
        if (customerFromDatabase == null) return NotFound();
        var customerToReturn = _mapper.Map<CustomerDto>(customerFromDatabase);
        return Ok(customerToReturn);
    }

    [HttpGet("with-addresses/{id}", Name = "GetCustomerWithAddressesById")]
    public ActionResult<CustomerWithAddressesDto> GetCustomerWithAddressesById(int id)
    {
        var customerFromDatabase = FindCustomerById(id);
        if (customerFromDatabase == null) return NotFound();
        var customerToReturn = _mapper.Map<CustomerDto>(customerFromDatabase);
        return Ok(customerToReturn);
    }

    [HttpGet("cpf/{cpf}")]
    public ActionResult<CustomerDto> GetCustomerByCpf(string cpf)
    {
        var customerFromDatabase = FindCustomerByCpf(cpf);
        if (customerFromDatabase == null) return NotFound();
        var customerToReturn = _mapper.Map<CustomerDto>(customerFromDatabase);
        return Ok(customerToReturn);
    }

    [HttpGet("with-addresses")]
    public ActionResult<IEnumerable<CustomerWithAddressesDto>> GetAllCustomersWithAddresses(int id)
    {
        var customerFromDatabase = _data.Customers;

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

        _mapper.Map(customerForUpdateDto, customerFromDatabase);

        return NoContent();
    }


    [HttpPut("with-addresses/{customerId}")]
    public ActionResult UpdateCustomerWithAddresses(int customerId,
       CustomerWithAddressesForUpdateDto customerWithAddressesForUpdateDto)
    {
        if (customerId != customerWithAddressesForUpdateDto.Id) return BadRequest();
        var customerFromDatabase = _data.Customers.FirstOrDefault(c => c.Id == customerId);
        if (customerFromDatabase == null) return NotFound();

        _mapper.Map(customerWithAddressesForUpdateDto, customerFromDatabase);


        var maxAddressId = _data.Customers.SelectMany(c => c.Addresses).Max(c => c.Id);
        customerFromDatabase.Addresses = customerWithAddressesForUpdateDto
                                        .Addresses.Select(
                                            address =>
                                            new Address()
                                            {
                                                Id = ++maxAddressId,
                                                City = address.City,
                                                Street = address.Street
                                            }
                                        ).ToList();

        return NoContent();
    }


    [HttpPatch("{id}")]
    public ActionResult PartiallyUpdateCustomer(
        [FromBody] JsonPatchDocument<CustomerForPatchDto> patchDocument,
        [FromRoute] int id)
    {
        var customerFromDatabase = _data.Customers
            .FirstOrDefault(customer => customer.Id == id);

        if (customerFromDatabase == null) return NotFound();

        var customerToPatch = new CustomerForPatchDto
        {
            Name = customerFromDatabase.Name,
            Cpf = customerFromDatabase.Cpf
        };

        patchDocument.ApplyTo(customerToPatch, ModelState);

        if(!TryValidateModel(customerToPatch))
        {
            return ValidationProblem(ModelState);
        }

        _mapper.Map(customerToPatch, customerFromDatabase);

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
        _data.Customers.Remove(customerFromDatabase);
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
        return _context.Customers.FirstOrDefault(c => c.Id == id)!;
    }

    private Customer FindCustomerByCpf(String cpf)
    {
        return _data.Customers.FirstOrDefault(c => c.Cpf == cpf)!;
    }




}

