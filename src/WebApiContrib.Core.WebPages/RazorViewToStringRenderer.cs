using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace WebApiContrib.Core.WebPages
{
    public class RazorViewToStringRenderer
    {
        private IRazorViewEngine _viewEngine;
        private ITempDataProvider _tempDataProvider;
        private IServiceProvider _serviceProvider;
        private IHttpContextAccessor _contextAccessor;

        public RazorViewToStringRenderer(
            IRazorViewEngine viewEngine,
            ITempDataProvider tempDataProvider,
            IServiceProvider serviceProvider,
            IHttpContextAccessor contextAccessor)
        {
            _viewEngine = viewEngine;
            _tempDataProvider = tempDataProvider;
            _serviceProvider = serviceProvider;
            _contextAccessor = contextAccessor;
        }

        public string RenderViewToString(string path)
        {
            var actionContext = GetActionContext();
            var viewEngineResult = _viewEngine.FindView(actionContext, path, isMainPage: true);

            if (!viewEngineResult.Success)
            {
                throw new InvalidOperationException(string.Format("Couldn't find view '{0}'", path));
            }

            var view = viewEngineResult.View;
            using (var output = new StringWriter())
            {
                var viewContext = new ViewContext(
                    actionContext,
                    view,
                    new ViewDataDictionary(
                        metadataProvider: new EmptyModelMetadataProvider(),
                        modelState: new ModelStateDictionary())
                    {},
                    new TempDataDictionary(actionContext.HttpContext, _tempDataProvider),
                    output,
                    new HtmlHelperOptions());

                view.RenderAsync(viewContext).GetAwaiter().GetResult();
                return output.ToString();
            }
        }

        private ActionContext GetActionContext()
        {
            _contextAccessor.HttpContext.RequestServices = _serviceProvider;
            return new ActionContext(_contextAccessor.HttpContext, new RouteData(), new ActionDescriptor());
        }
    }
}
