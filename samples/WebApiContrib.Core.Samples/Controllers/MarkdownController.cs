using Microsoft.AspNetCore.Mvc;
using WebApiContrib.Core.Samples.Model;

namespace WebApiContrib.Core.Samples.Controllers
{
    [Route("[controller]")]
    public class MarkdownController : Controller
    {
        [Route("sample")]
        public IActionResult Sample()
        {
            return View(new SampleMarkdownModel { Heading = "Hello!" });
        }
    }
}