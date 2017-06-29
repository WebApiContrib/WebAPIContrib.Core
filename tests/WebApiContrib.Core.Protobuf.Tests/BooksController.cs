using Microsoft.AspNetCore.Mvc;

namespace WebApiContrib.Core.Protobuf.Tests
{
    [Route("api/books")]
    public class BooksController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(Book.Data);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            if (Book.Data.Length >= id && id > 0)
            {
                return Ok(Book.Data[id-1]);
            }

            return Ok(null);
        }

        [HttpPost]
        public IActionResult Post([FromBody]Book book)
        {
            return Ok(book);
        }
    }
}
