using Microsoft.AspNetCore.Mvc;
using WebApiContrib.Core.Versioning;
using WebApiContrib.Core.Samples.Model;

namespace WebApiContrib.Core.Samples.Controllers
{
    [Route("api/versioned")]
    public class VersionedController : Controller
    {
        public IActionResult Get()
        {
            return new VersionedObjectResult(new PersonModel("Kristian", "Hellang", age: 27));
        }
    }
}