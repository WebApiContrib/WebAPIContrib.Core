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
            => context.WriteActionResult(new StatusCodeResult(statusCode));

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
            => context.WriteActionResult(new NoContentResult());

        public static Task Ok(this HttpContext context)
            => context.WriteActionResult(new OkResult());

        public static Task Ok(this HttpContext context, object value)
            => context.WriteActionResult(new OkObjectResult(value));

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

        public static Task RedirectToRoute(this HttpContext context, string routeName)
            => context.RedirectToRoute(routeName, routeValues: null);

        public static Task RedirectToRoute(this HttpContext context, object routeValues)
            => context.RedirectToRoute(routeName: null, routeValues: routeValues);

        public static Task RedirectToRoute(this HttpContext context, string routeName, object routeValues)
            => context.RedirectToRoute(routeName, routeValues, fragment: null);

        public static Task RedirectToRoute(this HttpContext context, string routeName, string fragment)
            => context.RedirectToRoute(routeName, routeValues: null, fragment: fragment);

        public static Task RedirectToRoute(this HttpContext context,
            string routeName,
            object routeValues,
            string fragment)
        {
            return context.WriteActionResult(new RedirectToRouteResult(routeName, routeValues, fragment)
            {
                UrlHelper = null,
            });
        }

        public static Task RedirectToRoutePreserveMethod(this HttpContext context,
            string routeName = null,
            object routeValues = null,
            string fragment = null)
        {
            return context.WriteActionResult(new RedirectToRouteResult(
                routeName: routeName,
                routeValues: routeValues,
                permanent: false,
                preserveMethod: true,
                fragment: fragment)
            {
                UrlHelper = null,
            });
        }

        public static Task RedirectToRoutePermanent(this HttpContext context, string routeName)
            => context.RedirectToRoutePermanent(routeName, routeValues: null);

        public static Task RedirectToRoutePermanent(this HttpContext context, object routeValues)
            => context.RedirectToRoutePermanent(routeName: null, routeValues: routeValues);

        public static Task RedirectToRoutePermanent(this HttpContext context, string routeName, object routeValues)
            => context.RedirectToRoutePermanent(routeName, routeValues, fragment: null);

        public static Task RedirectToRoutePermanent(this HttpContext context, string routeName, string fragment)
            => context.RedirectToRoutePermanent(routeName, routeValues: null, fragment: fragment);

        public static Task RedirectToRoutePermanent(this HttpContext context,
            string routeName,
            object routeValues,
            string fragment)
        {
            return context.WriteActionResult(new RedirectToRouteResult(routeName, routeValues, permanent: true, fragment: fragment)
            {
                UrlHelper = null,
            });
        }

        public static Task RedirectToRoutePermanentPreserveMethod(this HttpContext context,
            string routeName = null,
            object routeValues = null,
            string fragment = null)
        {
            return context.WriteActionResult(new RedirectToRouteResult(
                routeName: routeName,
                routeValues: routeValues,
                permanent: true,
                preserveMethod: true,
                fragment: fragment)
            {
                UrlHelper = null,
            });
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
            => context.WriteActionResult(new UnauthorizedResult());

        public static Task NotFound(this HttpContext context)
            => context.WriteActionResult(new NotFoundResult());

        public static Task NotFound(this HttpContext context, object value)
            => context.WriteActionResult(new NotFoundObjectResult(value));

        public static Task BadRequest(this HttpContext context)
            => context.WriteActionResult(new BadRequestResult());

        public static Task BadRequest(this HttpContext context, object error)
            => context.WriteActionResult(new BadRequestObjectResult(error));

        public static Task BadRequest(this HttpContext context, ModelStateDictionary modelState)
        {
            if (modelState == null)
            {
                throw new ArgumentNullException(nameof(modelState));
            }

            return context.WriteActionResult(new BadRequestObjectResult(modelState));
        }

        public static Task UnprocessableEntity(this HttpContext context)
            => context.WriteActionResult(new UnprocessableEntityResult());

        public static Task UnprocessableEntity(this HttpContext context, object error)
            => context.WriteActionResult(new UnprocessableEntityObjectResult(error));

        public static Task UnprocessableEntity(this HttpContext context, ModelStateDictionary modelState)
        {
            if (modelState == null)
            {
                throw new ArgumentNullException(nameof(modelState));
            }

            return context.WriteActionResult(new UnprocessableEntityObjectResult(modelState));
        }

        public static Task Conflict(this HttpContext context)
            => context.WriteActionResult(new ConflictResult());

        public static Task Conflict(this HttpContext context, object error)
            => context.WriteActionResult(new ConflictObjectResult(error));

        public static Task Conflict(this HttpContext context, ModelStateDictionary modelState)
            => context.WriteActionResult(new ConflictObjectResult(modelState));

        public static Task ValidationProblem(this HttpContext context, ValidationProblemDetails descriptor)
        {
            if (descriptor == null)
            {
                throw new ArgumentNullException(nameof(descriptor));
            }

            return context.WriteActionResult(new BadRequestObjectResult(descriptor));
        }

        public static Task ValidationProblem(this HttpContext context, ModelStateDictionary modelStateDictionary)
        {
            if (modelStateDictionary == null)
            {
                throw new ArgumentNullException(nameof(modelStateDictionary));
            }

            var validationProblem = new ValidationProblemDetails(modelStateDictionary);
            return context.WriteActionResult(new BadRequestObjectResult(validationProblem));
        }

        public static Task Created(this HttpContext context, string uri, object value)
        {
            if (uri == null)
            {
                throw new ArgumentNullException(nameof(uri));
            }

            return context.WriteActionResult(new CreatedResult(uri, value));
        }

        public static Task Created(this HttpContext context, Uri uri, object value)
        {
            if (uri == null)
            {
                throw new ArgumentNullException(nameof(uri));
            }

            return context.WriteActionResult(new CreatedResult(uri, value));
        }

        public static Task CreatedAtRoute(this HttpContext context, string routeName, object value)
            => context.CreatedAtRoute(routeName, routeValues: null, value: value);

        public static Task CreatedAtRoute(this HttpContext context, object routeValues, object value)
            => context.CreatedAtRoute(routeName: null, routeValues: routeValues, value: value);

        public static Task CreatedAtRoute(this HttpContext context, string routeName, object routeValues, object value)
            => context.WriteActionResult(new CreatedAtRouteResult(routeName, routeValues, value));

        public static Task Accepted(this HttpContext context)
            => context.WriteActionResult(new AcceptedResult());

        public static Task Accepted(this HttpContext context, object value)
            => context.WriteActionResult(new AcceptedResult(location: null, value: value));

        public static Task Accepted(this HttpContext context, Uri uri)
        {
            if (uri == null)
            {
                throw new ArgumentNullException(nameof(uri));
            }

            return context.WriteActionResult(new AcceptedResult(locationUri: uri, value: null));
        }

        public static Task Accepted(this HttpContext context, string uri)
            => context.WriteActionResult(new AcceptedResult(location: uri, value: null));
        
        public static Task Accepted(this HttpContext context, string uri, object value)
            => context.WriteActionResult(new AcceptedResult(uri, value));

        public static Task Accepted(this HttpContext context, Uri uri, object value)
        {
            if (uri == null)
            {
                throw new ArgumentNullException(nameof(uri));
            }

            return context.WriteActionResult(new AcceptedResult(locationUri: uri, value: value));
        }

        public static Task AcceptedAtRoute(this HttpContext context, object routeValues)
            => context.AcceptedAtRoute(routeName: null, routeValues: routeValues, value: null);

        public static Task AcceptedAtRoute(this HttpContext context, string routeName)
            => context.AcceptedAtRoute(routeName, routeValues: null, value: null);

        public static Task AcceptedAtRoute(this HttpContext context, string routeName, object routeValues)
            => context.AcceptedAtRoute(routeName, routeValues, value: null);

        public static Task AcceptedAtRoute(this HttpContext context, object routeValues, object value)
            => context.AcceptedAtRoute(routeName: null, routeValues: routeValues, value: value);

        public static Task AcceptedAtRoute(this HttpContext context, string routeName, object routeValues, object value)
            => context.WriteActionResult(new AcceptedAtRouteResult(routeName, routeValues, value));

        public static Task Challenge(this HttpContext context)
            => context.WriteActionResult(new ChallengeResult());

        public static Task Challenge(this HttpContext context, params string[] authenticationSchemes)
            => context.WriteActionResult(new ChallengeResult(authenticationSchemes));

        public static Task Challenge(this HttpContext context, AuthenticationProperties properties)
            => context.WriteActionResult(new ChallengeResult(properties));

        public static Task Challenge(this HttpContext context,
            AuthenticationProperties properties,
            params string[] authenticationSchemes)
            => context.WriteActionResult(new ChallengeResult(authenticationSchemes, properties));

        public static Task Forbid(this HttpContext context)
            => context.WriteActionResult(new ForbidResult());

        public static Task Forbid(this HttpContext context, params string[] authenticationSchemes)
            => context.WriteActionResult(new ForbidResult(authenticationSchemes));

        public static Task Forbid(this HttpContext context, AuthenticationProperties properties)
            => context.WriteActionResult(new ForbidResult(properties));

        public static Task Forbid(this HttpContext context, AuthenticationProperties properties, params string[] authenticationSchemes)
            => context.WriteActionResult(new ForbidResult(authenticationSchemes, properties));

        public static Task SignIn(this HttpContext context, ClaimsPrincipal principal, string authenticationScheme)
            => context.WriteActionResult(new SignInResult(authenticationScheme, principal));

        public static Task SignIn(this HttpContext context,
            ClaimsPrincipal principal,
            AuthenticationProperties properties,
            string authenticationScheme)
            => context.WriteActionResult(new SignInResult(authenticationScheme, principal, properties));

        public static Task SignOut(this HttpContext context, params string[] authenticationSchemes)
            => context.WriteActionResult(new SignOutResult(authenticationSchemes));

        public static Task SignOut(this HttpContext context, AuthenticationProperties properties, params string[] authenticationSchemes)
            => context.WriteActionResult(new SignOutResult(authenticationSchemes, properties));
    }
}