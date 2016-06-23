using Microsoft.AspNetCore.Mvc;
using WebApiContrib.Core.Samples.Model;

namespace WebApiContrib.Core.Samples.Controllers
{
    [Route("[controller]")]
    public class BindingController : Controller
    {
        [Route("")]
        [HttpPost]
        public IActionResult Post(SampleModel model)
        {
            return Ok(model);
        }
    }
}