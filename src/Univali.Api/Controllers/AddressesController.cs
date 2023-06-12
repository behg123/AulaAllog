using Microsoft.AspNetCore.Mvc;
using Univali.Api.Models;
using Univali.Api.Entities;
using AutoMapper;
using Univali.Api.DbContexts;
using Microsoft.EntityFrameworkCore;


namespace Univali.Api.Controllers;

[ApiController]
[Route("api/customers/{customerId}/addresses")]
public class AddressesController : MainController
{
    private readonly Data _data;
    private readonly IMapper _mapper;
    private readonly CustomerContext _context;

    public AddressesController(Data data, IMapper mapper, CustomerContext context)
    {
        _data = data ?? throw new ArgumentNullException(nameof(data));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _context = context ?? throw new ArgumentNullException(nameof(context));

    }

    //////////////////////////////////////////////////////////////////////////////
    //   ___ _ __ ___  __ _| |_ ___ 
    //  / __| '__/ _ \/ _` | __/ _ \
    // | (__| | |  __/ (_| | ||  __/
    //  \___|_|  \___|\__,_|\__\___|                        
    //////////////////////////////////////////////////////////////////////////////
    [HttpPost]
    public ActionResult<AddressDto> CreateAddress(int customerId, AddressForCreationDto addressForCreationDto)
    {
        var customerFromDatabase = FindCustomerById(customerId);
        if (customerFromDatabase == null) return NotFound();
        
        var addressEntity = _mapper.Map<Address>(addressForCreationDto);

        customerFromDatabase.Addresses.Add(addressEntity);

        _context.SaveChanges();

        var addresstoReturn = _mapper.Map<AddressDto>(addressEntity);

        return CreatedAtRoute
        (
            "GetAddressFromCustomer",
            new { customerId = customerId, addressId = addresstoReturn.Id },
            addresstoReturn
        );

    }

    //////////////////////////////////////////////////////////////////////////////
    //  _ __ ___  __ _  __| |
    // | '__/ _ \/ _` |/ _` |
    // | | |  __/ (_| | (_| |
    // |_|  \___|\__,_|\__,_|
    //////////////////////////////////////////////////////////////////////////////
    [HttpGet("{addressId}", Name = "GetAddressFromCustomer")]
    public ActionResult<AddressDto> GetAddressFromCustomer(int customerId, int addressId)
    {
        var customerFromDatabase = _data.Customers
            .FirstOrDefault(customer => customer.Id == customerId);

        if(customerFromDatabase == null) return NotFound();


        var addressFromDatabase = customerFromDatabase.Addresses
            .FirstOrDefault(address => address.Id == addressId);

        if (addressFromDatabase == null) return NotFound();

        var addresstoReturn = _mapper.Map<AddressDto>(addressFromDatabase);

        _context.SaveChanges();

        return Ok(addresstoReturn);
    }

    [HttpGet(Name = "GetAllAddressesFromCustomer")]
    public ActionResult<IEnumerable<AddressDto>> GetAllAddressesFromCustomer(int customerId)
    {
        var customerFromDatabase = _context.Customers
            .Include(c => c.Addresses)
            .FirstOrDefault(c => c.Id == customerId);

        if (customerFromDatabase == null) return NotFound();

        var addressToReturn = _mapper.Map<List<AddressDto>>(customerFromDatabase.Addresses);

        _context.SaveChanges();

        return Ok(addressToReturn);
    }



    //////////////////////////////////////////////////////////////////////////////
    //  _   _ _ __   __| | __ _| |_ ___ 
    // | | | | '_ \ / _` |/ _` | __/ _ \
    // | |_| | |_) | (_| | (_| | ||  __/
    //  \__,_| .__/ \__,_|\__,_|\__\___|
    //       |_|                        
    //////////////////////////////////////////////////////////////////////////////
    [HttpPut("{addressId}")]
    public ActionResult UpdateAddressFromCustomer(int customerId, int addressId, AddressForUpdateDto addressForUpdateDto)
    {
        var addressFromCustomer = FindAddressById(customerId, addressId);
        if (addressFromCustomer == null) return NotFound();

        _mapper.Map(addressForUpdateDto, addressFromCustomer);

        _context.SaveChanges();

        return NoContent();
    }



    [HttpDelete("{addressId}")]
    public ActionResult DeleteAddressFromCustomer(int customerId, int addressId)
    {
        var customer = FindCustomerById(customerId);
        if (customer == null)
            return NotFound();

        var address = FindAddressById(customerId, addressId);
        if (address == null)
            return NotFound();

        customer.Addresses.Remove(address);

        _context.SaveChanges();

        return NoContent();
    }

    //////////////////////////////////////////////////////////////////////////////              
    //  _   _| |_(_) |___ 
    // | | | | __| | / __|
    // | |_| | |_| | \__ \
    //  \__,_|\__|_|_|___/
    //////////////////////////////////////////////////////////////////////////////      
    private Customer FindCustomerById(int id)
    {
        return _context.Customers.FirstOrDefault(c => c.Id == id)!;
    }

    private Address FindAddressById(int customerId, int addressId)
    {
        var customerFromDatabase = _data.Customers
            .FirstOrDefault(customer => customer.Id == customerId);
        if (customerFromDatabase == null) return null!;
        return customerFromDatabase.Addresses.FirstOrDefault(address => address.Id == addressId)!;
    }

    private Customer FindCustomerByCpf(String cpf)
    {
        return _context.Customers.FirstOrDefault(c => c.Cpf == cpf)!;
    }

}