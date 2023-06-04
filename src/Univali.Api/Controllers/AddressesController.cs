using Microsoft.AspNetCore.Mvc;
using Univali.Api.Models;
using Univali.Api.Entities;

namespace Univali.Api.Controllers;

[ApiController]
[Route("api/customers/{customerId}/addresses")]
public class AdressControler : ControllerBase
{

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
        IEnumerable<Address> allAddresses = Data.instanceAcess().Customers.SelectMany(customer => customer.Addresses);

        var addressEntity = new Address
        {
            Id = allAddresses.Max(c => c.Id) + 1,
            Street = addressForCreationDto.Street,
            City = addressForCreationDto.City
        };

        customerFromDatabase.Addresses.Add(addressEntity);

        var addresstoReturn = ConvertToAddressDto(addressEntity);

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
    [HttpGet]
    public ActionResult<IEnumerable<AddressDto>> GetAllAdressesFromCustomer(int customerId)
    {
        var customerFromDatabase = FindCustomerById(customerId);
        if (customerFromDatabase == null) return NotFound();
        var addressToReturn = new List<AddressDto>();
        foreach (var address in customerFromDatabase.Addresses)
        {
            addressToReturn.Add(new AddressDto
            {
                Id = address.Id,
                City = address.City,
                Street = address.Street
            });
        }
        return Ok(addressToReturn);
    }


    [HttpGet("{addressId}", Name = "GetAddressFromCustomer")]
    public ActionResult<AddressDto> GetAddressFromCustomer(int customerId, int addressId)
    {
        var addressFromCustomer = FindAddressById(customerId, addressId);

        if (addressFromCustomer == null) return NotFound();
 
        AddressDto addressToReturn = ConvertToAddressDto(addressFromCustomer);

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
    public ActionResult<AddressDto> UpdateAddressFromCustomer(int customerId, int addressId, AddressForUpdateDto AddressForUpdateDto)
    {

        var address = FindAddressById(customerId, addressId);
        if (address == null) return NotFound();

        address.Street = AddressForUpdateDto.Street;
        address.City = AddressForUpdateDto.City;

        var addressToReturn = new AddressDto
        {
            Id = address.Id,
            Street = address.Street,
            City = address.City
        };

        return Ok(addressToReturn);
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
        return Data.instanceAcess().Customers.FirstOrDefault(c => c.Id == id)!;
    }

    private Address FindAddressById(int customerId, int addressId)
    {
        var customerEntity = Data.instanceAcess().Customers.FirstOrDefault(c => c.Id == customerId)!;
        if (customerEntity == null) return null;
        return customerEntity.Addresses.FirstOrDefault(a => a.Id == addressId)!;
    }


    private Customer FindCustomerByCpf(String cpf)
    {
        return Data.instanceAcess().Customers.FirstOrDefault(c => c.Cpf == cpf)!;
    }

    private AddressDto ConvertToAddressDto(Address address)
    {
        var addressDto = new AddressDto
        {
            Id = address.Id,
            Street = address.Street,
            City = address.City
        };

        return addressDto;
    }


    private Address ConvertToAddress(AddressDto addressDto)
    {
        var adressConverted = new Address
        {
            Id = addressDto.Id,
            Street = addressDto.Street,
            City = addressDto.City
        };

        return adressConverted;
    }






}