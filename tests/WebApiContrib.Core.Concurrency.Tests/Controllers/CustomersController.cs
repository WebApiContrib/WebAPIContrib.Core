#region copyright
// Copyright 2016 WebApiContrib
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiContrib.Core.Concurrency.Tests.Models;

namespace WebApiContrib.Core.Concurrency.Tests.Controllers
{
    [Route("customers")]
    public class CustomersController : Controller
    {
        private const string EntityName = "customer_";

        private readonly IRepresentationManager _representationManager;

        private List<Customer> _customers;

        #region Constructor

        public CustomersController(IRepresentationManager representationManager)
        {
            _customers = new List<Customer>();
            _representationManager = representationManager;
        }

        #endregion

        #region Public methods

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
            if (!await _representationManager.CheckRepresentationAsync(this, EntityName + customer.CustomerId))
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

        #endregion
    }
}
