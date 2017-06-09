using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebApiContrib.Core.Results
{
    public class JsonStringResult : IActionResult
    {
        private readonly string _jsonString;

        public JsonStringResult(string jsonString)
        {
            _jsonString = jsonString;
        }

        public Task ExecuteResultAsync(ActionContext context)
        {
            context.HttpContext.Response.StatusCode = 200;
            context.HttpContext.Response.ContentType = "application/json";
            return context.HttpContext.Response.WriteAsync(_jsonString);
        }
    }
}
