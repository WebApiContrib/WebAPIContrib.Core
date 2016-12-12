using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace WebApiContrib.Core.WebPages
{
    // adapted, with Imran's permission, from https://weblogs.asp.net/imranbaloch/adding-web-pages-in-aspnet-core
    public class RazorViewToStringRenderer
    {
        private readonly IRazorViewEngine _viewEngine;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RazorViewToStringRenderer(IRazorViewEngine viewEngine, ITempDataProvider tempDataProvider,
            IHttpContextAccessor httpContextAccessor)
        {
            _viewEngine = viewEngine;
            _tempDataProvider = tempDataProvider;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> RenderViewToString(string path)
        {
            var actionContext = GetActionContext();
            try
            {
                var viewEngineResult = _viewEngine.FindView(actionContext, path, isMainPage: false);

                if (!viewEngineResult.Success)
                {
                    throw new InvalidOperationException($"Couldn't find view '{path}'");
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
                        { },
                        new TempDataDictionary(actionContext.HttpContext, _tempDataProvider),
                        output,
                        new HtmlHelperOptions());

                    await view.RenderAsync(viewContext).ConfigureAwait(false);
                    return output.ToString();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }
        }

        private ActionContext GetActionContext()
        {
            return new ActionContext(_httpContextAccessor.HttpContext, _httpContextAccessor.HttpContext.GetRouteData(), new ActionDescriptor());
        }
    }
}
