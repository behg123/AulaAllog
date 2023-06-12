using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Univali.Api.DbContexts;
using Univali.Api.Entities;
using Univali.Api.Models;
using Univali.Api.Repositores;
using Univali.Api.Features.Customers.Queries.GetCustomerDetail;
using Univali.Api.Features.Customers.Commands.CreateCustomer;

namespace Univali.Api.Controllers;

[ApiController]
[Route("Api/customers")]
public class CustomersController : MainController
{

    private readonly Data _data;
    private readonly IMapper _mapper;
    private readonly CustomerContext _context;
    private readonly ICustomerRepository _customerRepository;

    public CustomersController(Data data, IMapper mapper, CustomerContext context, ICustomerRepository customerRepository)
    {
        _data = data ?? throw new ArgumentNullException(nameof(data));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
        
    }

    ///////////////////////////////////////
    //   ___ _ __ ___  __ _| |_ ___ 
    //  / __| '__/ _ \/ _` | __/ _ \
    // | (__| | |  __/ (_| | ||  __/
    //  \___|_|  \___|\__,_|\__\___|                        
    ///////////////////////////////////////
    [HttpPost]
    public async Task<ActionResult<CustomerDto>> CreateCustomer(CreateCustomerCommand createCustomerCommand, [FromServices] ICreateCustomerCommandHandler handler)
    {
        var customerToReturn = await handler.Handle(createCustomerCommand);
        
        return CreatedAtRoute("GetCustomerById", new { customerId = customerToReturn.Id },customerToReturn);
    }

    [HttpPost("with-addresses")]
    public ActionResult<CustomerDto> CreateCustomerWithAddresses(CustomerWithAddressesDto customerWithAddressesDto)
    {
        var customerEntity = _mapper.Map<Customer>(customerWithAddressesDto);

        foreach (var addressDto in customerWithAddressesDto.Addresses)
        {
            var address = _mapper.Map<Address>(addressDto);
            customerEntity.Addresses.Add(address);
        }

        _context.Customers.Add(customerEntity);
        var customerToReturn = _mapper.Map<CustomerDto>(customerEntity);

        _context.SaveChanges();

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
    // |_|  \___|\__,_|\__,_|   |
    ///////////////////////////////////////
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CustomerDto>>> GetCustomers()
    {
        var customerFromDatabase = await _customerRepository.GetCustomersAsync();
        var customerDtos = _mapper.Map<IEnumerable<CustomerDto>>(customerFromDatabase);     
        return Ok(customerDtos);

    }

    [HttpGet("{customerId}", Name = "GetCustomerById")]
    public async Task<ActionResult<CustomerDto>> GetCustomerById([FromServices] IGetCustomerDetailQueryHandler handler, int customerId)
    {
        var getCustomerDetailQuery = new GetCustomerDetailQuery {Id = customerId};
 
        var customerToReturn = await handler.Handle(getCustomerDetailQuery);
        //var customerFromDatabase = await _customerRepository.GetCustomersAsync();
        if (customerToReturn == null) return NotFound();
        //var customerToReturn = _mapper.Map<CustomerDto>(customerFromDatabase);
        return Ok(customerToReturn);
    }

    [HttpGet("with-addresses/{id}", Name = "GetCustomerWithAddressesById")]
    public async Task<ActionResult<CustomerWithAddressesDto>> GetCustomerWithAddressesById(int id)
    {
        var customerFromDatabase = await _customerRepository.GetCustomersAsync();
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
    public ActionResult<IEnumerable<CustomerWithAddressesDto>> GetCustomersWithAddresses()
    {

        var customersFromDatabase = _context.Customers.Include(c => c.Addresses).ToList();

        var customersToReturn = _mapper.Map<IEnumerable<CustomerWithAddressesDto>>(customersFromDatabase);

        return Ok(customersToReturn);
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
        _context.SaveChanges();

        return NoContent();
    }


    // [HttpPut("with-addresses/{customerId}")]
    // public ActionResult UpdateCustomerWithAddresses(int customerId, CustomerWithAddressesForUpdateDto customerWithAddressesForUpdateDto)
    // {
    //     if (customerId != customerWithAddressesForUpdateDto.Id) return BadRequest();

    //     var customerFromDatabase = _customerRepository.GetCustomerById(customerId);
    //     if (customerFromDatabase == null) return NotFound();

    //     _mapper.Map(customerWithAddressesForUpdateDto, customerFromDatabase);
    //     _context.SaveChanges();

    //     _mapper.Map(customerWithAddressesForUpdateDto, customerFromDatabase);
    //     _customerRepository.UpdateCustomerAsync(customerFromDatabase);

    //     return NoContent();
    // }

    


    [HttpPatch("{id}")]
    public ActionResult PartiallyUpdateCustomer(
        [FromBody] JsonPatchDocument<CustomerForPatchDto> patchDocument,
        [FromRoute] int id)
    {
        var customerFromDatabase = _context.Customers
            .FirstOrDefault(customer => customer.Id == id);

        if (customerFromDatabase == null) return NotFound();

        var customerToPatch = _mapper.Map<CustomerForPatchDto>(customerFromDatabase);

        patchDocument.ApplyTo(customerToPatch, ModelState);

        if (!TryValidateModel(customerToPatch))
        {
            return ValidationProblem(ModelState);
        }

        _mapper.Map(customerToPatch, customerFromDatabase);

        _context.SaveChanges();

        return NoContent();

    }
    ///////////////////////////////////////
    //   __| | ___| | ___| |_ ___ 
    //  / _` |/ _ \ |/ _ \ __/ _ \
    // | (_| |  __/ |  __/ ||  __/
    //  \__,_|\___|_|\___|\__\___|
    ///////////////////////////////////////                 
    [HttpDelete("{id}")]
    public  async Task<ActionResult<CustomerDto>> DeleteCustomer(int id)
    {
        var deleted = await _customerRepository.DeleteCustomerAsync(id);
        if (!deleted) return NotFound();

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
        return _context.Customers.FirstOrDefault(c => c.Cpf == cpf)!;
    }




}

