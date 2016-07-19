using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiContrib.Core.Concurrency;
using WebApiContrib.Core.Concurrency.Integration.Tests.Models;

namespace WebApiContrib.Core.Integration.Tests.Controllers
{
    [Route("customers")]
    public class CustomersController : Controller
    {
        private const string EntityName = "customer_";

        private readonly IRepresentationManager _representationManager;

        private List<Customer> _customers;

        public CustomersController(IRepresentationManager representationManager)
        {
            _customers = new List<Customer>();
            _representationManager = representationManager;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Customer customer)
        {
            customer.CustomerId = Guid.NewGuid().ToString();
            _customers.Add(customer);
            await _representationManager.AddOrUpdateRepresentationAsync(this, EntityName + customer.CustomerId);
            return new OkObjectResult(customer);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Customer customer)
        {
            if (!await _representationManager.CheckRepresentationExistsAsync(this, EntityName + customer.CustomerId))
            {
                return new ContentResult
                {
                    StatusCode = 412
                };
            }

            // Update the customer & update the representation
            await _representationManager.AddOrUpdateRepresentationAsync(this, EntityName + customer.CustomerId);
            return NoContent();
        }

        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            return Ok();
        }
    }
}
