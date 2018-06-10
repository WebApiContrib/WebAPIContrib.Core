using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using WebApiContrib.Core.Formatter.Csv;
using WebApiContrib.Core.Samples.Model;

namespace WebApiContrib.Core.Samples.Controllers
{
    /// <summary>
    /// 
    /// Configuration is used for both CSV Output AND Input formatters
    /// 
    /// 1. Only primitive value type properties are allowed for UseProperty (no reference types and no method calls are allowed)
    /// 2. Chain parameterless ForHeader() method when:
    ///     a) Not using headers (UseProperty is always chained after UseHeader)
    ///     b) Using headers but you want them generated automatically based on property name (or path)
    /// 3. Chain method UseCsvDelimiter(string) when you want to override default delimiter (semilocolon)
    /// 4. Chain method UseEncoding when you want to override default encoding (ISO-8859-1)
    /// 5. Chain method UseFormatProvider when you want to provide custom formatting for your primitive types
    /// 
    /// </summary>
    public class AuthorModelConfiguration : IFormattingConfiguration<AuthorModel>
    {
        public void Configure(IFormattingConfigurationBuilder<AuthorModel> builder)
        {
            CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");
            DateTimeFormatInfo dtfi = culture.DateTimeFormat;
            dtfi.DateSeparator = "-";
            builder
                .UseHeaders()
                .UseFormatProvider(culture)
                .ForHeader("Identifier")
                .UseProperty(x => x.Id)
                .ForHeader("First Name")
                .UseProperty(x => x.FirstName)
                .ForHeader("Last Name")
                .UseProperty(x => x.LastName)
                .ForHeader("Date of Birth")
                .UseProperty(x => x.DateOfBirth)
                .ForHeader("IQ")
                .UseProperty(x => x.IQ)
                .ForHeader("Street")
                .UseProperty(x => x.Address.Street)
                .ForHeader("City")
                .UseProperty(x => x.Address.City)
                // Header name will be inferred from property path 'Address.City'
                .ForHeader()
                .UseProperty(x => x.Address.Country)
                // Header name will be inferred from property name 'Signature'
                .ForHeader()
                .UseProperty(x => x.Signature);
        }
    }

    [Route("api/[controller]")]
    public class FluentCsvTestController : Controller
    {
        // GET api/fluentcsvtest
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(DummyDataList());
        }

        [HttpGet]
        [Route("data.csv")]
        [Produces("text/csv")]
        public IActionResult GetDataAsCsv()
        {
            return Ok(DummyDataList());
        }

        [HttpGet]
        [Route("dataarray.csv")]
        [Produces("text/csv")]
        public IActionResult GetArrayDataAsCsv()
        {
            return Ok(DummyDataArray());
        }

        private static IEnumerable<AuthorModel> DummyDataList()
        {
            return new List<AuthorModel>
            {
                new AuthorModel
                {
                    Id = 1,
                    FirstName = "Joanne",
                    LastName = "Rowling",
                    DateOfBirth = DateTime.Now,
                    IQ = 70,
                    Signature = "signature",
                    Address = new AuthorAddress
                    {
                        Street = null,
                        City = "London",
                        Country = "UK"
                    }
                },
                new AuthorModel
                {
                    Id = 1,
                    FirstName = "Hermann",
                    LastName = "Hesse",
                    DateOfBirth = DateTime.Now,
                    IQ = 180,
                    Signature = "signature"
                }
            };
        }

        private static AuthorModel[] DummyDataArray()
        {
            return new AuthorModel[]
            {
                new AuthorModel
                {
                    Id = 1,
                    FirstName = "Joanne",
                    LastName = "Rowling",
                    DateOfBirth = DateTime.Now,
                    IQ = 70,
                    Signature = "signature",
                    Address = new AuthorAddress
                    {
                        Street = null,
                        City = "London",
                        Country = "UK"
                    }
                },
                new AuthorModel
                {
                    Id = 1,
                    FirstName = "Hermann",
                    LastName = "Hesse",
                    DateOfBirth = DateTime.Now,
                    IQ = 180,
                    Signature = "signature",
                    Address = new AuthorAddress
                    {
                        Street = null,
                        City = "Berlin",
                        Country = "Germany"
                    }
                }
            };
        }
    }
}