using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Razor.Internal;
using Microsoft.Extensions.FileProviders;

namespace WebApiContrib.Core.WebPages
{
    // adapted, with Imran's permission, from https://weblogs.asp.net/imranbaloch/adding-web-pages-in-aspnet-core
    public class WebPagesRouter : IRouter
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly RazorViewToStringRenderer _renderer;
        private readonly WebPagesOptions _opts;
        private readonly IRazorViewEngineFileProviderAccessor _fileProviderAccessor;

        public WebPagesRouter(IHostingEnvironment hostingEnvironment, RazorViewToStringRenderer renderer, WebPagesOptions opts, IRazorViewEngineFileProviderAccessor fileProviderAccessor)
        {
            if (opts == null) throw new ArgumentNullException(nameof(opts));

            _hostingEnvironment = hostingEnvironment;
            _renderer = renderer;
            _opts = opts;
            _fileProviderAccessor = fileProviderAccessor;
        }

        public VirtualPathData GetVirtualPath(VirtualPathContext context)
        {
            return null;
        }

        public async Task RouteAsync(RouteContext context)
        {
            var path = context.HttpContext.Request.Path.ToString().TrimStart('/').TrimEnd('/');

            // root
            if (path == string.Empty && _opts.RootViewName != null)
            {
                path = _opts.RootViewName;
            }

            if (!path.Contains(".")) // if path doesn't have an extension, we want to probe it for being a page
            {
                var filePath = Path.Combine(_hostingEnvironment.ContentRootPath, _opts.ViewsFolderName, path + ".cshtml");
                var fileInfo = _fileProviderAccessor.FileProvider.GetFileInfo(filePath);
                if (fileInfo == null)
                {
                    context.HttpContext.Response.StatusCode = 404;
                    return;
                }

                var contents = await _renderer.RenderViewToString(path);
                await context.HttpContext.Response.WriteAsync(contents);
            }
        }
    }
}
