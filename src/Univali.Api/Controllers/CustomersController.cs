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
        var result = new List<Customer>
            {
                new Customer{
                    Id = 1,
                    Name = "Joao",
                    Cpf = "128975641"
                },
                new Customer{
                    Id = 2,
                    Name = "Renan",
                    Cpf = "751478625"
                }
            };

        return result;
    }
}

