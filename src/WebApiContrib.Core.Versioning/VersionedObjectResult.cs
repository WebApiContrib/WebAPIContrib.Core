using Microsoft.AspNetCore.Mvc;

namespace WebApiContrib.Core.Versioning
{
    public class VersionedObjectResult : ObjectResult
    {
        public VersionedObjectResult(object value) : base(value)
        {
        }
    }
}