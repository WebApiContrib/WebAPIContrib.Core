using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

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

    public class Book
    {
        public static Book[] Data = new[]
            {
                new Book { Title = "Our Mathematical Universe: My Quest for the Ultimate Nature of Reality", Author = "Max Tegmark"},
                new Book { Title = "Hockey Towns", Author = "Ron MacLean"},
            };

        public string Title { get; set; }

        public string Author { get; set; }
    }
}
