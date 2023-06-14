using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Univali.Api.DbContexts;
using Univali.Api.Entities;
using Univali.Api.Models;
using Univali.Api.Repositories;
using Univali.Api.Features.Customers.Commands.CreateCustomer;
using Univali.Api.Features.Queries.GetCustomers;
using Univali.Api.Features.Customers.Queries.GetCustomerDetail;
using MediatR;
using Univali.Api.Features.Customers.Queries.GetCustomerWithAddresses;
using Univali.Api.Features.Queries.GetCustomerByCpf;
using Univali.Api.Features.Queries.GetCustomersWithAddresses;
using Univali.Api.Features.Commands.UpdateCustomer;
using Univali.Api.Features.Commands.CreateCustomer.DeleteCustomer;
using Microsoft.AspNetCore.Authorization;
using Univali.Api.Features.Commands.UpdateCustomerWithAddresses;

namespace Univali.Api.Controllers;

[ApiController]
[Route("Api/customers")]
[Authorize]
public class CustomersController : MainController
{
    private readonly Data _data;
    private readonly IMapper _mapper;
    private readonly CustomerContext _context;
    private readonly ICustomerRepository _customerRepository;
    private readonly IMediator _mediator;

    public CustomersController(Data data, IMapper mapper, CustomerContext context,
        ICustomerRepository customerRepository, IMediator mediator)
    {
        _data = data ?? throw new ArgumentNullException(nameof(data));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }
    ///////////////////////////////////////
    //   ___ _ __ ___  __ _| |_ ___ 
    //  / __| '__/ _ \/ _` | __/ _ \
    // | (__| | |  __/ (_| | ||  __/
    //  \___|_|  \___|\__,_|\__\___|                        
    ///////////////////////////////////////
    [HttpPost]
    public async Task<ActionResult<CustomerDto>> CreateCustomer(CreateCustomerCommand createCustomerCommand)
    {
        var customerToReturn = await _mediator.Send(createCustomerCommand);
        return CreatedAtRoute("GetCustomerById", new { customerId = customerToReturn.Id }, customerToReturn);
    }


    [HttpPost("with-addresses")]
    public async Task<ActionResult<CustomerDto>> CreateCustomerWithAddresses(CreateCustomerWithAddressesCommand createCustomerWithAddressesCommand)
    {
        var customerToReturn = await _mediator.Send(createCustomerWithAddressesCommand);
        return CreatedAtRoute("GetCustomerWithAddressesById", new { id = customerToReturn.Id }, customerToReturn);
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
        var query = new GetCustomersQuery();
        var customerDtos = await _mediator.Send(query);
        return Ok(customerDtos);
    }

    [HttpGet("{customerId}", Name = "GetCustomerById")]
    public async Task<ActionResult<CustomerDto>> GetCustomerById(int customerId)
    {
        var getCustomerDetailQuery = new GetCustomerDetailQuery { Id = customerId };
        var customerToReturn = await _mediator.Send(getCustomerDetailQuery);
        if (customerToReturn == null) return NotFound();
        return Ok(customerToReturn);
    }

    [HttpGet("with-addresses/{id}", Name = "GetCustomerWithAddressesById")]
    public async Task<ActionResult<CustomerWithAddressesDto>> GetCustomerWithAddressesById(int id)
    {
        var query = new GetCustomerWithAddressesQuery { Id = id };
        var customerWithAddresses = await _mediator.Send(query);
        if (customerWithAddresses == null) return NotFound();
        return Ok(customerWithAddresses);
    }

    [HttpGet("cpf/{cpf}")]
    public async Task<ActionResult<CustomerDto>> GetCustomerByCpf(string cpf)
    {
        var query = new GetCustomerByCpfQuery { Cpf = cpf };
        var customerDto = await _mediator.Send(query);

        if (customerDto == null)
            return NotFound();

        return Ok(customerDto);
    }

    [HttpGet("with-addresses")]
    public async Task<ActionResult<IEnumerable<CustomerDto>>> GetCustomersWithAddresses()
    {
        var query = new GetCustomersWithAddressesQuery();
        var customersWithAddresses = await _mediator.Send(query);

        return Ok(customersWithAddresses);
    }

    ///////////////////////////////////////
    //  _   _ _ __   __| | __ _| |_ ___ 
    // | | | | '_ \ / _` |/ _` | __/ _ \
    // | |_| | |_) | (_| | (_| | ||  __/
    //  \__,_| .__/ \__,_|\__,_|\__\___|
    //       |_|                        
    ///////////////////////////////////////
    [HttpPut("{id}")]
    public async Task<ActionResult<IEnumerable<CustomerDto>>> UpdateCustomer(int id, CustomerForUpdateDto customerForUpdateDto)
    {
        if (id != customerForUpdateDto.Id)
            return BadRequest();

        var updateCustomerCommand = _mapper.Map<UpdateCustomerCommand>(customerForUpdateDto);
        updateCustomerCommand.Id = id;

        var updatedCustomer = await _mediator.Send(updateCustomerCommand);

        if (updatedCustomer == null)
        {
            return NotFound();
        }

        return NoContent();
    }


    [HttpPut("with-addresses/{customerId}")]
    public async Task<ActionResult> UpdateCustomerWithAddresses(int customerId, CustomerWithAddressesForUpdateDto customerWithAddressesForUpdateDto)
    {
        if (customerId != customerWithAddressesForUpdateDto.Id) return BadRequest();

        var updateCustomerWithAddressesCommand = _mapper.Map<UpdateCustomerWithAddressesCommand>(customerWithAddressesForUpdateDto);
        updateCustomerWithAddressesCommand.Id = customerId;

        var updatedCustomer = await _mediator.Send(updateCustomerWithAddressesCommand);

        if (updatedCustomer == null)
        {
            return NotFound();
        }

        return NoContent();
    }



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
    public async Task<ActionResult<CustomerDto>> DeleteCustomer(int id)
    {
        var deleteCustomerCommand = new DeleteCustomerCommand { Id = id };
        var deleted = await _mediator.Send(deleteCustomerCommand);

        if (deleted == null)
            return NotFound();

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

