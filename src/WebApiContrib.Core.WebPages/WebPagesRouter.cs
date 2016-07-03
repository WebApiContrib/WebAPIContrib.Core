using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebApiContrib.Core.WebPages
{
    public class WebPagesRouter : IRouter
    {
        private IHostingEnvironment _hostingEnvironment;
        private RazorViewToStringRenderer _renderer;

        public WebPagesRouter(IHostingEnvironment hostingEnvironment, RazorViewToStringRenderer renderer)
        {
            _hostingEnvironment = hostingEnvironment;
            _renderer = renderer;
        }

        public VirtualPathData GetVirtualPath(VirtualPathContext context)
        {
            return null;
        }

        public async Task RouteAsync(RouteContext context)
        {
            var path = context.HttpContext.Request.Path.ToString().TrimStart('/');

            if (Regex.IsMatch(path, "^([\\w]+/)*[\\w]+[.]cshtml$"))
            {
                var filePath = Path.Combine(_hostingEnvironment.ContentRootPath, path);

                //context.IsHandled = true;
                if (!File.Exists(filePath))
                {
                    context.HttpContext.Response.StatusCode = 404;
                    return;
                }

                var contents = _renderer.RenderViewToString("~/" + path);
                await context.HttpContext.Response.WriteAsync(contents);
            }
        }
    }
}
