using Microsoft.AspNetCore.Mvc;
using WebApiContrib.Core.Formatter.Integration.Tests.Models;

namespace WebApiContrib.Core.Formatter.Integration.Tests.Controllers
{
    [Route("customers")]
    public class CustomersController : Controller
    {
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            var customer = new Customer
            {
                CustomerId = id,
                FirstName = "Loki"
            };

            return new OkObjectResult(customer);
        }

        [HttpPost]
        public IActionResult Post(Customer customer)
        {
            return Ok();
        }
    }
}
