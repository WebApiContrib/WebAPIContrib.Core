# Import and Export CSV in ASP.NET Core

WebApiContrib.Core.Formatter.Csv [![NuGet Status](http://img.shields.io/nuget/v/WebApiContrib.Core.Formatter.Csv.svg?style=flat-square)](https://www.nuget.org/packages/WebApiContrib.Core.Formatter.Csv/)

# History

2018.06.10: Adding support for fluent based configuration of input and output formatters
2018.04.18: Adding support for customization of the header with the display attribute
2018.04.12: Using the encoding from the options in the CsvOutputFormatter, Don't buffer CSV 
2017.02.14: update to csproj
2016.06.22: project init

# Documentation

The InputFormatter and the OutputFormatter classes are used to convert the csv data to/from the C# model classes.

**Aside from that, there are two types of formatters (standard and fluent), which differ in the way they work and get configured––both are described in the following sections.**

 **Code sample:** https://github.com/WebApiContrib/WebAPIContrib.Core/tree/master/samples/WebApiContrib.Core.Samples

## Fluent Formatters


Fluent formatters use lambda expressions to generate csv (output) or models from incoming csv (input). They support multi-level object hierarchy and can be hooked to any class having public properties–in the sample project, the AuthorModel and AddressModel are used as an example.

```csharp
using System;

namespace WebApiContrib.Core.Samples.Model
{
    public class AuthorModel
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int IQ { get; set; }
        public object Signature { get; set; }

        public AuthorAddress Address { get; set; }
    }

    public class AuthorAddress
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
}

```

The MVC Controller **FluentCsvTestController** makes it possible to import and export the data. The Get method exports the data using the Accept header in the HTTP Request. Per default, Json will be returned. If the Accept Header is set to 'text/csv', the data will be returned as csv. The GetDataAsCsv method always returns csv data because the Produces attribute is used to force this. This makes it easy to download the csv data in a browser. 

The Import method uses the Content-Type HTTP Request header to decide how to handle the request body. If the 'text/csv' is defined, the custom csv input formatter will be used.

**AuthorModelConfiguration** defines the configuration and a single set of lambda expressions that will be used for **both** input and output formatters.

When you want to define a new configuration, implement IFormattingConfiguration interface respecting the following guidelines:

1. Only primitive value type properties are allowed for UseProperty (no reference types and no method calls are allowed)
2. Chain parameterless ForHeader() method when:
 - Not using headers (UseProperty is always chained after UseHeader)
 - Using headers but you want them generated automatically based on property name (or path)
3. Chain method UseCsvDelimiter(string) when you want to override default delimiter (semilocolon)
4. Chain method UseEncoding when you want to override default encoding (ISO-8859-1)
5. Chain method UseFormatProvider when you want to provide custom formatting for your primitive types

```csharp
using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using WebApiContrib.Core.Formatter.Csv;
using WebApiContrib.Core.Samples.Model;

namespace WebApiContrib.Core.Samples.Controllers
{
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

```

The formatters can be added to the ASP.NET Core project in the Startup class in the ConfigureServices method.

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddCsvSerializerFormatters(
        builder =>
        {
            builder.RegisterConfiguration(new AuthorModelConfiguration());
        })
}

```

**Note: As opposed to standard formatters, the fluent formatters cannot be configured directly.**

When the data.csv link is requested, a csv type response is returned to the client, which can be saved. This data contains the header texts and the value of each property in each object. This can then be opened in excel.

http://localhost:10336/api/fluentcsvtest/data.csv

```csharp
Identifier;First Name;Last Name;Date of Birth;IQ;Street;City;Address.Country;Signature
1;Joanne;Rowling;6-10-18 11:12:10 AM;70;;London;UK;signature
1;Hermann;Hesse;6-10-18 11:12:10 AM;180;;;;signature
```

## Standard Formatters

Standard formatters use reflection to generate csv based on models (output) or models from incoming csv (input). They support only one level of object hierarchy and thus has limited usages. It also requires the creation of a DTO that will 'carry' the data. In the sample project, the LocalizationRecord class is used as a DTO to import and export to and from csv data.

You can customize header with the **DisplayAttribute**.

```csharp
using System.ComponentModel.DataAnnotations;

namespace WebApiContrib.Core.Samples.Model
{
    public class LocalizationRecord
    {
        public long Id { get; set; }
        public string Key { get; set; }
        public string Text { get; set; }
        public string LocalizationCulture { get; set; }
        public string ResourceKey { get; set; }

        [Display(Name = "Value")]
        public string ResourceValue { get; set; }
    }
}

```

The MVC Controller **CsvTestController** makes it possible to import and export the data. The Get method exports the data using the Accept header in the HTTP Request. Per default, Json will be returned. If the Accept Header is set to 'text/csv', the data will be returned as csv. The GetDataAsCsv method always returns csv data because the Produces attribute is used to force this. This makes it easy to download the csv data in a browser. 

The Import method uses the Content-Type HTTP Request header to decide how to handle the request body. If the 'text/csv' is defined, the custom csv input formatter will be used.

```csharp
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using WebApiContrib.Core.Samples.Model;

namespace WebApiContrib.Core.Samples.Controllers
{
    [Route("api/[controller]")]
    public class CsvTestController : Controller
    {
        // GET api/csvtest
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
            return Ok( DummyDataList());
        }

        private static IEnumerable<LocalizationRecord> DummyDataList()
        {
            var model = new List<LocalizationRecord>
            {
                new LocalizationRecord
                {
                    Id = 1,
                    Key = "test",
                    Text = "test text",
                    LocalizationCulture = "en-US",
                    ResourceKey = "test",
                    ResourceValue = "test value"

                },
                new LocalizationRecord
                {
                    Id = 2,
                    Key = "test",
                    Text = "test2 text de-CH",
                    LocalizationCulture = "de-CH",
                    ResourceKey = "test",
                    ResourceValue = "test value"
                }
            };

            return model;
        }

        // POST api/csvtest/import
        [HttpPost]
        [Route("import")]
        public IActionResult Import([FromBody]List<LocalizationRecord> value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                List<LocalizationRecord> data = value;
                return Ok();
            }
        }
    }
}

```

The formatters can be added to the ASP.NET Core project in the Startup class in the ConfigureServices method. The code configuration accepts an options object to define the delimiter and if a single line header should be included in the csv file or not.

The default delimiter is set to ';' and the header is included by default.

```csharp
public void ConfigureServices(IServiceCollection services)
{
	//var csvOptions = new CsvFormatterOptions
	//{
	//    UseSingleLineHeaderInCsv = true,
	//    CsvDelimiter = ","
	//};

	//services.AddMvc()
	//    .AddCsvSerializerFormatters(csvOptions);

	services.AddMvc()
	   .AddCsvSerializerFormatters();
}
```

The custom formatters can also be configured directly. The Content-Type 'text/csv' is used for the csv formatters. 

```csharp
public void ConfigureServices(IServiceCollection services)
{
	var csvFormatterOptions = new CsvFormatterOptions();

	services.AddMvc(options =>
	{
		options.InputFormatters.Add(new StandardCsvInputFormatter(csvFormatterOptions));
		options.OutputFormatters.Add(new StandardCsvOutputFormatter(csvFormatterOptions));
		options.FormatterMappings.SetMediaTypeMappingForFormat("csv", MediaTypeHeaderValue.Parse("text/csv"));
	});
}
```

When the data.csv link is requested, a csv type response is returned to the client, which can be saved. This data contains the header texts and the value of each property in each object. This can then be opened in excel.

http://localhost:10336/api/csvtest/data.csv

```csharp
Id;Key;Text;LocalizationCulture;ResourceKey
1;test;test text;en-US;test
2;test;test2 text de-CH;de-CH;test
```

This data can then be used to upload the csv data to the server which is then converted back to a C# object. I use fiddler, postman or curl can also be used, or any HTTP Client where you can set the header Content-Type.

```csharp

 http://localhost:10336/api/csvtest/import 

 User-Agent: Fiddler 
 Content-Type: text/csv 
 Host: localhost:10336 
 Content-Length: 110 


 Id;Key;Text;LocalizationCulture;ResourceKey 
 1;test;test text;en-US;test 
 2;test;test2 text de-CH;de-CH;test 

```

The following image shows that the data is imported correctly.


<img src="https://damienbod.files.wordpress.com/2016/06/importexportcsv.png" alt="importExportCsv" width="598" height="558" class="alignnone size-full wp-image-6742" />

<strong>Notes</strong>

The implementation of the InputFormatter and the OutputFormatter classes are specific for a list of simple classes with only properties. If you require or use more complex classes, these implementations need to be changed.

<strong>Links</strong>

http://www.tugberkugurlu.com/archive/creating-custom-csvmediatypeformatter-in-asp-net-web-api-for-comma-separated-values-csv-format

https://damienbod.com/2015/06/03/asp-net-5-mvc-6-custom-protobuf-formatters/

http://www.strathweb.com/2014/11/formatters-asp-net-mvc-6/

https://wildermuth.com/2016/03/16/Content_Negotiation_in_ASP_NET_Core
