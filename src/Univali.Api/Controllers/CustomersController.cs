using Microsoft.AspNetCore.Mvc;
using Unvali.Api.Entities;

namespace Univali.Api.Controllers;

[ApiController]
[Route("Api/customers")]
public class CustomersController : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<Customer>> GetCustomers()
    {

        var result = Data.instanceAcess().Customers;


        return Ok(result);
    }
}

