# WebAPIContrib for ASP.NET CORE

|                           | Badges                                                                                                                                                       |
| ------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| net451 & netstandard 1.6        | [![Build status](https://ci.appveyor.com/api/projects/status/4n10t3rrkju3fwyy?svg=true)](https://ci.appveyor.com/project/thabart/webapicontrib-core)         |

WebAPIContrib.Core is a collection of open source projects, add-ons and extensions to help improve your work with ASP.NET Core and ASP.NET Core MVC.

## Main

* [WebApiContrib.Core](https://github.com/WebApiContrib/WebAPIContrib.Core/tree/master/src/WebApiContrib.Core)
  * `GlobalRoutePrefixConvention` - `IApplicationModelConvention` allowing you to set a global route prefix, which is then combined into all actions
  * `FromBodyApplicationModelConvention` - `IApplicationModelConvention` allowing you to globally apply body binding source to action parameters. You can also provide predicates to filter on specific controllers, actions or parameters
  * `OverridableFilterProvider` - allows you to override filters from higher scope (i.e. global filters) on lower scope (i.e. controller filters)
  * `ValidationAttribute` - an action filter returning 400 response in case there are any model state errors

## Formatters

* [WebApiContrib.Core.Formatter.Bson](https://github.com/WebApiContrib/WebAPIContrib.Core/tree/master/src/WebApiContrib.Core.Formatter.Bson)
* [WebApiContrib.Core.Formatter.Csv](https://github.com/WebApiContrib/WebAPIContrib.Core/tree/master/src/WebApiContrib.Core.Formatter.Csv)
* [WebApiContrib.Core.Formatter.PlainText](https://github.com/WebApiContrib/WebAPIContrib.Core/tree/master/src/WebApiContrib.Core.Formatter.PlainText)

## TagHelpers
* [WebApiContrib.Core.TagHelpers.Markdown](https://github.com/WebApiContrib/WebAPIContrib.Core/tree/master/src/WebApiContrib.Core.TagHelpers.Markdown)

## Other

* [WebApiContrib.Core.Concurrency](https://github.com/WebApiContrib/WebAPIContrib.Core/tree/master/src/WebApiContrib.Core.Concurrency)
* [WebApiContrib.Core.Concurrency.Redis](https://github.com/WebApiContrib/WebAPIContrib.Core/tree/master/src/WebApiContrib.Core.Concurrency.Redis)
* [WebApiContrib.Core.Concurrency.SqlServer](https://github.com/WebApiContrib/WebAPIContrib.Core/tree/master/src/WebApiContrib.Core.Concurrency.SqlServer)

---

### Contributing
If you would like to contribute, feel free to fork the projects or get in touch with the mailing list: https://groups.google.com/group/webapicontrib or on [Slack](https://webapicontrib.azurewebsites.net). Also make sure to look at the [contributing guidelines](https://github.com/WebApiContrib/WebAPIContrib.Core/blob/dev/CONTRIBUTING.md).

### Want to transfer your project to WebApiContrib?

You created your own project and now want to transfer it to WebApiContrib? Awesome! [We've got you covered](https://github.com/WebApiContrib/WebAPIContrib/wiki/Guidelines-for-transferring-projects-to-Web-API-Contrib). It is easy peasy.
