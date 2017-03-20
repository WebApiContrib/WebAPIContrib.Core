using System;
using Microsoft.AspNetCore.Http;

namespace WebApiContrib.Core
{
    public static class HttpContextExtensions
    {
        public static T RegisterForDispose<T>(this T disposable, HttpContext context) where T : IDisposable
        {
            context.Response.RegisterForDispose(disposable);
            return disposable;
        }
    }
}