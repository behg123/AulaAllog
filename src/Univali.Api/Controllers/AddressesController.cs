using Microsoft.AspNetCore.Mvc;
using Univali.Api.Models;

namespace Univali.Api.Controllers; 

[ApiController]
[Route("api/customers/{customerId}/addresses")]
public class AdressControler : ControllerBase{
    [HttpGet]
    public ActionResult<IEnumerable<AddressDto>> GetAdresses(int customerId){
        var customerFromDatabase = Data.instanceAcess().Customers.FirstOrDefault(customer => customer.Id == customerId);
        if(customerFromDatabase == null) return NotFound();
        var addressToReturn = new List<AddressDto>();
        foreach(var address in customerFromDatabase.Addresses){
            addressToReturn.Add(new AddressDto{
                Id = address.Id,
                City = address.City,
                Street = address.Street
            });
        }
        return Ok(addressToReturn);
    }

    [HttpGet]
    public ActionResult<AddressDto> GetAddress(int customerID, int addressId){
        var addresstoReturn = Data.instanceAcess().Customers.FirstOrDefault(customer => customer.Id == customerID)?.Addresses.FirstOrDefault(address => address.Id == addressId);

        return addresstoReturn != null ? Ok(addresstoReturn) : NotFound();


    }
}