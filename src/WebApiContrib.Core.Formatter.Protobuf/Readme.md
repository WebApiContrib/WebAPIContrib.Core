
https://www.nuget.org/packages/WebApiContrib.Core.Formatter.Protobuf

# Getting Started

```
 services.AddControllers(options =>
{
    options.FormatterMappings
        .SetMediaTypeMappingForFormat("protobuf", 
          MediaTypeHeaderValue.Parse("application/x-protobuf"));
}).AddProtobufFormatters();
```

# History

2020-08-28 Released Version 3.0.0

- Update protobuf-net 3.0.29
  - PR https://github.com/WebApiContrib/WebAPIContrib.Core/pull/210