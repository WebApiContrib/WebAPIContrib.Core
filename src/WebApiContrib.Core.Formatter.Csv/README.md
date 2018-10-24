# Import and Export CSV in ASP.NET Core

WebApiContrib.Core.Formatter.Csv [![NuGet Status](http://img.shields.io/nuget/v/WebApiContrib.Core.Formatter.Csv.svg?style=flat-square)](https://www.nuget.org/packages/WebApiContrib.Core.Formatter.Csv/)

## History

- 2018.10.24: Support IEnumerable instead of ILIst for the OutputFormatter, OrderBy and format fix
- 2018.09.14: Added correct usage of CanRead/CanReadType
- 2018.05.31: Adding support for ignoring propeties in the CSV DTO
- 2018.04.18: Adding support for customization of the header with the display attribute
- 2018.04.12: Using the encoding from the options in the CsvOutputFormatter, Don't buffer CSV 
- 2017.02.14: update to csproj
- 2016.06.22: project init

## Documentation

The InputFormatter and the OutputFormatter classes are used to convert the csv data to the C# model classes. 

 **Code sample:** https://github.com/WebApiContrib/WebAPIContrib.Core/tree/master/samples/WebApiContrib.Core.Samples

The LocalizationRecord class is used as the model class to import and export to and from csv data.

You can customize header with the  **DisplayAttribute**.

```csharp
using System.ComponentModel.DataAnnotations;

namespace WebApiContrib.Core.Samples.Model
{
    public class LocalizationRecord
    {
        [JsonIgnore]
        public long? Id { get; set; }

        [JsonProperty(PropertyName = "CustomKeyName")]
        public string Key { get; set; }

        public string Text { get; set; }

        public string LocalizationCulture { get; set; }
        
        public string ResourceKey { get; set; }
    }
}

```

The MVC Controller CsvTestController  makes it possible to import and export the data. The Get method exports the data using the Accept header in the HTTP Request. Per default, Json will be returned. If the Accept Header is set to 'text/csv', the data will be returned as csv. The GetDataAsCsv method always returns csv data because the Produces attribute is used to force this. This makes it easy to download the csv data in a browser. 

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

Optionally setting IncludeExcelDelimiterHeader to true adds the "sep=,"  header, so the files can be opened in Excel directly.

```csharp
public void ConfigureServices(IServiceCollection services)
{
	//var csvOptions = new CsvFormatterOptions
	//{
	//    UseSingleLineHeaderInCsv = true,
	//    CsvDelimiter = ",",
	//    IncludeExcelDelimiterHeader = true
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
		options.InputFormatters.Add(new CsvInputFormatter(csvFormatterOptions));
		options.OutputFormatters.Add(new CsvOutputFormatter(csvFormatterOptions));
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

## Notes

The implementation of the InputFormatter and the OutputFormatter classes are specific for a list of simple classes with only properties. If you require or use more complex classes, these implementations need to be changed.

## Links

http://www.tugberkugurlu.com/archive/creating-custom-csvmediatypeformatter-in-asp-net-web-api-for-comma-separated-values-csv-format

https://damienbod.com/2015/06/03/asp-net-5-mvc-6-custom-protobuf-formatters/

http://www.strathweb.com/2014/11/formatters-asp-net-mvc-6/

https://wildermuth.com/2016/03/16/Content_Negotiation_in_ASP_NET_Core
