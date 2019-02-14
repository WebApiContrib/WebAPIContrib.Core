using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using System;
using System.IO;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace WebApiContrib.Core.Results
{
    public static class HttpContextExtensions
    {
        public static Task WriteActionResult<TResult>(this HttpContext context, TResult result) where TResult : IActionResult
        {
            var executor = context.RequestServices.GetService<IActionResultExecutor<TResult>>();

            if (executor == null)
            {
                throw new InvalidOperationException($"No action result executor for {typeof(TResult).FullName} registered.");
            }

            var routeData = context.GetRouteData() ?? new RouteData();
            var actionContext = new ActionContext(context, routeData, new ActionDescriptor());

            return executor.ExecuteAsync(actionContext, result);
        }

        public static Task StatusCode(this HttpContext context, int statusCode)
        {
            context.Response.StatusCode = statusCode;
            return context.Response.Body.FlushAsync();
        }

        public static Task StatusCode(this HttpContext context, int statusCode, object value)
        {
            var result = new ObjectResult(value)
            {
                StatusCode = statusCode
            };

            return context.WriteActionResult(result);
        }

        public static Task Content(this HttpContext context, string content)
            => context.Content(content, (MediaTypeHeaderValue)null);

        public static Task Content(this HttpContext context, string content, string contentType)
            => context.Content(content, MediaTypeHeaderValue.Parse(contentType));

        public static Task Content(this HttpContext context, string content, string contentType, Encoding contentEncoding)
        {
            var mediaTypeHeaderValue = MediaTypeHeaderValue.Parse(contentType);
            mediaTypeHeaderValue.Encoding = contentEncoding ?? mediaTypeHeaderValue.Encoding;
            return context.Content(content, mediaTypeHeaderValue);
        }

        public static Task Content(this HttpContext context, string content, MediaTypeHeaderValue contentType)
        {
            var result = new ContentResult
            {
                Content = content,
                ContentType = contentType?.ToString()
            };

            return context.WriteActionResult(result);
        }

        public static Task NoContent(this HttpContext context)
            => context.StatusCode(StatusCodes.Status204NoContent);

        public static Task Ok(this HttpContext context)
            => context.StatusCode(StatusCodes.Status200OK);

        public static Task Ok(this HttpContext context, object value)
            => context.WriteActionResult(new ObjectResult(value)
            {
                StatusCode = StatusCodes.Status200OK
            });

        public static Task Redirect(this HttpContext context, string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentException(nameof(url));
            }

            return context.WriteActionResult(new RedirectResult(url));
        }

        public static Task RedirectPermanent(this HttpContext context, string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentException(nameof(url));
            }

            return context.WriteActionResult(new RedirectResult(url, permanent: true));
        }

        public static Task RedirectPreserveMethod(this HttpContext context, string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentException(nameof(url));
            }

            return context.WriteActionResult(new RedirectResult(url: url, permanent: false, preserveMethod: true));
        }

        public static Task RedirectPermanentPreserveMethod(this HttpContext context, string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentException(nameof(url));
            }

            return context.WriteActionResult(new RedirectResult(url: url, permanent: true, preserveMethod: true));
        }

        public static Task LocalRedirect(this HttpContext context, string localUrl)
        {
            if (string.IsNullOrEmpty(localUrl))
            {
                throw new ArgumentException(nameof(localUrl));
            }

            return context.WriteActionResult(new LocalRedirectResult(localUrl));
        }

        public static Task LocalRedirectPermanent(this HttpContext context, string localUrl)
        {
            if (string.IsNullOrEmpty(localUrl))
            {
                throw new ArgumentException(nameof(localUrl));
            }

            return context.WriteActionResult(new LocalRedirectResult(localUrl, permanent: true));
        }

        public static Task LocalRedirectPreserveMethod(this HttpContext context, string localUrl)
        {
            if (string.IsNullOrEmpty(localUrl))
            {
                throw new ArgumentException(nameof(localUrl));
            }

            return context.WriteActionResult(new LocalRedirectResult(localUrl: localUrl, permanent: false, preserveMethod: true));
        }

        public static Task LocalRedirectPermanentPreserveMethod(this HttpContext context, string localUrl)
        {
            if (string.IsNullOrEmpty(localUrl))
            {
                throw new ArgumentException(nameof(localUrl));
            }

            return context.WriteActionResult(new LocalRedirectResult(localUrl: localUrl, permanent: true, preserveMethod: true));
        }

        public static Task File(this HttpContext context, byte[] fileContents, string contentType)
            => context.File(fileContents, contentType, fileDownloadName: null);

        public static Task File(this HttpContext context, byte[] fileContents, string contentType, bool enableRangeProcessing)
            => context.File(fileContents, contentType, fileDownloadName: null, enableRangeProcessing: enableRangeProcessing);

        public static Task File(this HttpContext context, byte[] fileContents, string contentType, string fileDownloadName)
            => context.WriteActionResult(new FileContentResult(fileContents, contentType) { FileDownloadName = fileDownloadName });

        public static Task File(this HttpContext context, byte[] fileContents, string contentType, string fileDownloadName, bool enableRangeProcessing)
            => context.WriteActionResult(new FileContentResult(fileContents, contentType)
            {
                FileDownloadName = fileDownloadName,
                EnableRangeProcessing = enableRangeProcessing,
            });

        public static Task File(this HttpContext context, byte[] fileContents, string contentType, DateTimeOffset? lastModified, EntityTagHeaderValue entityTag)
            => context.WriteActionResult(new FileContentResult(fileContents, contentType)
            {
                LastModified = lastModified,
                EntityTag = entityTag,
            });

        public static Task File(this HttpContext context, byte[] fileContents, string contentType, DateTimeOffset? lastModified, EntityTagHeaderValue entityTag, bool enableRangeProcessing)
            => context.WriteActionResult(new FileContentResult(fileContents, contentType)
            {
                LastModified = lastModified,
                EntityTag = entityTag,
                EnableRangeProcessing = enableRangeProcessing,
            });

        public static Task File(this HttpContext context, byte[] fileContents, string contentType, string fileDownloadName, DateTimeOffset? lastModified, EntityTagHeaderValue entityTag)
            => context.WriteActionResult(new FileContentResult(fileContents, contentType)
            {
                LastModified = lastModified,
                EntityTag = entityTag,
                FileDownloadName = fileDownloadName,
            });

        public static Task File(this HttpContext context, byte[] fileContents, string contentType, string fileDownloadName, DateTimeOffset? lastModified, EntityTagHeaderValue entityTag, bool enableRangeProcessing)
            => context.WriteActionResult(new FileContentResult(fileContents, contentType)
            {
                LastModified = lastModified,
                EntityTag = entityTag,
                FileDownloadName = fileDownloadName,
                EnableRangeProcessing = enableRangeProcessing,
            });

        public static Task File(this HttpContext context, Stream fileStream, string contentType)
            => context.File(fileStream, contentType, fileDownloadName: null);

        public static Task File(this HttpContext context, Stream fileStream, string contentType, bool enableRangeProcessing)
            => context.File(fileStream, contentType, fileDownloadName: null, enableRangeProcessing: enableRangeProcessing);

        public static Task File(this HttpContext context, Stream fileStream, string contentType, string fileDownloadName)
            => context.WriteActionResult(new FileStreamResult(fileStream, contentType) { FileDownloadName = fileDownloadName });

        public static Task File(this HttpContext context, Stream fileStream, string contentType, string fileDownloadName, bool enableRangeProcessing)
            => context.WriteActionResult(new FileStreamResult(fileStream, contentType)
            {
                FileDownloadName = fileDownloadName,
                EnableRangeProcessing = enableRangeProcessing,
            });

        public static Task File(this HttpContext context, Stream fileStream, string contentType, DateTimeOffset? lastModified, EntityTagHeaderValue entityTag)
            => context.WriteActionResult(new FileStreamResult(fileStream, contentType)
            {
                LastModified = lastModified,
                EntityTag = entityTag,
            });

        public static Task File(this HttpContext context, Stream fileStream, string contentType, DateTimeOffset? lastModified, EntityTagHeaderValue entityTag, bool enableRangeProcessing)
            => context.WriteActionResult(new FileStreamResult(fileStream, contentType)
            {
                LastModified = lastModified,
                EntityTag = entityTag,
                EnableRangeProcessing = enableRangeProcessing,
            });

        public static Task File(this HttpContext context, Stream fileStream, string contentType, string fileDownloadName, DateTimeOffset? lastModified, EntityTagHeaderValue entityTag)
            => context.WriteActionResult(new FileStreamResult(fileStream, contentType)
            {
                LastModified = lastModified,
                EntityTag = entityTag,
                FileDownloadName = fileDownloadName,
            });

        public static Task File(this HttpContext context, Stream fileStream, string contentType, string fileDownloadName, DateTimeOffset? lastModified, EntityTagHeaderValue entityTag, bool enableRangeProcessing)
            => context.WriteActionResult(new FileStreamResult(fileStream, contentType)
            {
                LastModified = lastModified,
                EntityTag = entityTag,
                FileDownloadName = fileDownloadName,
                EnableRangeProcessing = enableRangeProcessing,
            });

        public static Task File(this HttpContext context, string virtualPath, string contentType)
            => context.File(virtualPath, contentType, fileDownloadName: null);

        public static Task File(this HttpContext context, string virtualPath, string contentType, bool enableRangeProcessing)
            => context.File(virtualPath, contentType, fileDownloadName: null, enableRangeProcessing: enableRangeProcessing);

        public static Task File(this HttpContext context, string virtualPath, string contentType, string fileDownloadName)
            => context.WriteActionResult(new VirtualFileResult(virtualPath, contentType) { FileDownloadName = fileDownloadName });

        public static Task File(this HttpContext context, string virtualPath, string contentType, string fileDownloadName, bool enableRangeProcessing)
            => context.WriteActionResult(new VirtualFileResult(virtualPath, contentType)
            {
                FileDownloadName = fileDownloadName,
                EnableRangeProcessing = enableRangeProcessing,
            });

        public static Task File(this HttpContext context, string virtualPath, string contentType, DateTimeOffset? lastModified, EntityTagHeaderValue entityTag)
            => context.WriteActionResult(new VirtualFileResult(virtualPath, contentType)
            {
                LastModified = lastModified,
                EntityTag = entityTag,
            });

        public static Task File(this HttpContext context, string virtualPath, string contentType, DateTimeOffset? lastModified, EntityTagHeaderValue entityTag, bool enableRangeProcessing)
            => context.WriteActionResult(new VirtualFileResult(virtualPath, contentType)
            {
                LastModified = lastModified,
                EntityTag = entityTag,
                EnableRangeProcessing = enableRangeProcessing,
            });

        public static Task File(this HttpContext context, string virtualPath, string contentType, string fileDownloadName, DateTimeOffset? lastModified, EntityTagHeaderValue entityTag)
            => context.WriteActionResult(new VirtualFileResult(virtualPath, contentType)
            {
                LastModified = lastModified,
                EntityTag = entityTag,
                FileDownloadName = fileDownloadName,
            });

        public static Task File(this HttpContext context, string virtualPath, string contentType, string fileDownloadName, DateTimeOffset? lastModified, EntityTagHeaderValue entityTag, bool enableRangeProcessing)
            => context.WriteActionResult(new VirtualFileResult(virtualPath, contentType)
            {
                LastModified = lastModified,
                EntityTag = entityTag,
                FileDownloadName = fileDownloadName,
                EnableRangeProcessing = enableRangeProcessing,
            });

        public static Task PhysicalFile(this HttpContext context, string physicalPath, string contentType)
            => context.PhysicalFile(physicalPath, contentType, fileDownloadName: null);

        public static Task PhysicalFile(this HttpContext context, string physicalPath, string contentType, bool enableRangeProcessing)
            => context.PhysicalFile(physicalPath, contentType, fileDownloadName: null, enableRangeProcessing: enableRangeProcessing);

        public static Task PhysicalFile(this HttpContext context,
            string physicalPath,
            string contentType,
            string fileDownloadName)
            => context.WriteActionResult(new PhysicalFileResult(physicalPath, contentType) { FileDownloadName = fileDownloadName });

        public static Task PhysicalFile(this HttpContext context,
            string physicalPath,
            string contentType,
            string fileDownloadName,
            bool enableRangeProcessing)
            => context.WriteActionResult(new PhysicalFileResult(physicalPath, contentType)
            {
                FileDownloadName = fileDownloadName,
                EnableRangeProcessing = enableRangeProcessing,
            });

        public static Task PhysicalFile(this HttpContext context, string physicalPath, string contentType, DateTimeOffset? lastModified, EntityTagHeaderValue entityTag)
            => context.WriteActionResult(new PhysicalFileResult(physicalPath, contentType)
            {
                LastModified = lastModified,
                EntityTag = entityTag,
            });

        public static Task PhysicalFile(this HttpContext context, string physicalPath, string contentType, DateTimeOffset? lastModified, EntityTagHeaderValue entityTag, bool enableRangeProcessing)
            => context.WriteActionResult(new PhysicalFileResult(physicalPath, contentType)
            {
                LastModified = lastModified,
                EntityTag = entityTag,
                EnableRangeProcessing = enableRangeProcessing,
            });

        public static Task PhysicalFile(this HttpContext context, string physicalPath, string contentType, string fileDownloadName, DateTimeOffset? lastModified, EntityTagHeaderValue entityTag)
            => context.WriteActionResult(new PhysicalFileResult(physicalPath, contentType)
            {
                LastModified = lastModified,
                EntityTag = entityTag,
                FileDownloadName = fileDownloadName,
            });

        public static Task PhysicalFile(this HttpContext context, string physicalPath, string contentType, string fileDownloadName, DateTimeOffset? lastModified, EntityTagHeaderValue entityTag, bool enableRangeProcessing)
            => context.WriteActionResult(new PhysicalFileResult(physicalPath, contentType)
            {
                LastModified = lastModified,
                EntityTag = entityTag,
                FileDownloadName = fileDownloadName,
                EnableRangeProcessing = enableRangeProcessing,
            });

        public static Task Unauthorized(this HttpContext context)
            => context.StatusCode(StatusCodes.Status401Unauthorized);

        public static Task Unauthorized(this HttpContext context, object value)
            => context.WriteActionResult(new ObjectResult(value)
            {
                StatusCode = StatusCodes.Status401Unauthorized
            });

        public static Task NotFound(this HttpContext context)
            => context.StatusCode(StatusCodes.Status404NotFound);

        public static Task NotFound(this HttpContext context, object value)
            => context.WriteActionResult(new ObjectResult(value)
            {
                StatusCode = StatusCodes.Status404NotFound
            });

        public static Task BadRequest(this HttpContext context)
            => context.StatusCode(StatusCodes.Status400BadRequest);

        public static Task BadRequest(this HttpContext context, object error)
            => context.WriteActionResult(new ObjectResult(error)
            {
                StatusCode = StatusCodes.Status400BadRequest
            });

        public static Task BadRequest(this HttpContext context, ModelStateDictionary modelState)
        {
            if (modelState == null)
            {
                throw new ArgumentNullException(nameof(modelState));
            }

            return context.BadRequest(modelState);
        }

        public static Task UnprocessableEntity(this HttpContext context)
            => context.StatusCode(StatusCodes.Status422UnprocessableEntity);

        public static Task UnprocessableEntity(this HttpContext context, object error)
            => context.WriteActionResult(new ObjectResult(error)
            {
                StatusCode = StatusCodes.Status422UnprocessableEntity
            });

        public static Task UnprocessableEntity(this HttpContext context, ModelStateDictionary modelState)
        {
            if (modelState == null)
            {
                throw new ArgumentNullException(nameof(modelState));
            }

            return context.WriteActionResult(new ObjectResult(modelState)
            {
                StatusCode = StatusCodes.Status422UnprocessableEntity
            });
        }

        public static Task Conflict(this HttpContext context)
            => context.StatusCode(StatusCodes.Status409Conflict);

        public static Task Conflict(this HttpContext context, object error)
            => context.WriteActionResult(new ObjectResult(error)
            {
                StatusCode = StatusCodes.Status409Conflict
            });

        public static Task Conflict(this HttpContext context, ModelStateDictionary modelState)
            => context.WriteActionResult(new ObjectResult(modelState)
            {
                StatusCode = StatusCodes.Status409Conflict
            });

        public static Task ValidationProblem(this HttpContext context, ValidationProblemDetails descriptor)
        {
            if (descriptor == null)
            {
                throw new ArgumentNullException(nameof(descriptor));
            }

            return context.WriteActionResult(new ObjectResult(descriptor)
            {
                StatusCode = StatusCodes.Status400BadRequest
            });
        }

        public static Task ValidationProblem(this HttpContext context, ModelStateDictionary modelStateDictionary)
        {
            if (modelStateDictionary == null)
            {
                throw new ArgumentNullException(nameof(modelStateDictionary));
            }

            var validationProblem = new ValidationProblemDetails(modelStateDictionary);
            return context.WriteActionResult(new ObjectResult(validationProblem)
            {
                StatusCode = StatusCodes.Status400BadRequest
            });
        }

        public static Task Created(this HttpContext context, string uri, object value)
        {
            if (uri == null)
            {
                throw new ArgumentNullException(nameof(uri));
            }

            context.Response.Headers.Add("Location", uri);
            return context.WriteActionResult(new ObjectResult(value)
            {
                StatusCode = StatusCodes.Status201Created
            });
        }

        public static Task Created(this HttpContext context, Uri uri, object value)
        {
            if (uri == null)
            {
                throw new ArgumentNullException(nameof(uri));
            }

            context.Response.Headers.Add("Location", uri.ToString());
            return context.WriteActionResult(new ObjectResult(value)
            {
                StatusCode = StatusCodes.Status201Created
            });
        }

        public static Task Accepted(this HttpContext context)
            => context.StatusCode(StatusCodes.Status202Accepted);

        public static Task Accepted(this HttpContext context, object value)
        {
            return context.WriteActionResult(new ObjectResult(value)
            {
                StatusCode = StatusCodes.Status202Accepted
            });
        }

        public static Task Accepted(this HttpContext context, Uri uri)
        {
            if (uri == null)
            {
                throw new ArgumentNullException(nameof(uri));
            }

            context.Response.Headers.Add("Location", uri.ToString());
            return context.StatusCode(StatusCodes.Status202Accepted);
        }

        public static Task Accepted(this HttpContext context, string uri)
        {
            context.Response.Headers.Add("Location", uri);
            return context.StatusCode(StatusCodes.Status202Accepted);
        }
        
        public static Task Accepted(this HttpContext context, string uri, object value)
        {
            context.Response.Headers.Add("Location", uri);
            return context.WriteActionResult(new ObjectResult(value)
            {
                StatusCode = StatusCodes.Status202Accepted
            });
        }

        public static Task Accepted(this HttpContext context, Uri uri, object value)
        {
            if (uri == null)
            {
                throw new ArgumentNullException(nameof(uri));
            }

            context.Response.Headers.Add("Location", uri.ToString());
            return context.WriteActionResult(new ObjectResult(value)
            {
                StatusCode = StatusCodes.Status202Accepted
            });
        }

        public static Task Forbid(this HttpContext context)
            => context.StatusCode(StatusCodes.Status403Forbidden);
    }
}