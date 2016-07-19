using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiContrib.Core.Concurrency.Extensions;

namespace WebApiContrib.Core.Concurrency
{
    public interface IRepresentationManager
    {
        Task AddOrUpdateRepresentationAsync(
            Controller controller, 
            string representationId);

        Task UpdateHeader(
            Controller controller,
            string representationId);

        Task<bool> CheckRepresentationExistsAsync(
            Controller controller,
            string representationId);

        Task<bool> CheckRepresentationHasChangedAsync(
            Controller controller,
            string representationId);
    }

    internal class RepresentationManager : IRepresentationManager
    {
        private readonly IConcurrencyManager _concurrencyManager;
             
        public RepresentationManager(IConcurrencyManager concurrencyManager)
        {
            _concurrencyManager = concurrencyManager;
        }
        
        public async Task AddOrUpdateRepresentationAsync(
            Controller controller, 
            string representationId)
        {
            if (controller == null)
            {
                throw new ArgumentNullException(nameof(controller));
            }

            if (string.IsNullOrWhiteSpace(representationId))
            {
                throw new ArgumentNullException(nameof(representationId));
            }

            var concurrentObject = await _concurrencyManager.TryUpdateRepresentationAsync(representationId);
            SetHeaders(controller, concurrentObject);
        }

        public async Task UpdateHeader(
            Controller controller,
            string representationId)
        {
            if (controller == null)
            {
                throw new ArgumentNullException(nameof(controller));
            }

            if (string.IsNullOrWhiteSpace(representationId))
            {
                throw new ArgumentNullException(nameof(representationId));
            }

            var concurrentObject = await _concurrencyManager.TryGetRepresentationAsync(representationId);
            if (concurrentObject == null)
            {
                return;
            }

            SetHeaders(controller, concurrentObject);
        }

        public async Task<bool> CheckRepresentationExistsAsync(
            Controller controller,
            string representationId)
        {
            if (controller == null)
            {
                throw new ArgumentNullException(nameof(controller));
            }

            if (string.IsNullOrWhiteSpace(representationId))
            {
                throw new ArgumentNullException(nameof(representationId));
            }

            var concatenatedEtags = controller.GetIfMatch();
            var unmodifiedSince = controller.GetUnmodifiedSince();
            var checkDateCallback = new Func<DateTime, ConcurrentObject, bool>((d, c) =>
            {
                return c.DateTime <= d;
            });
            var checkEtagCorrectCallback = new Func<ConcurrentObject, List<EntityTagHeaderValue>, bool>((c, etags) =>
            {
                return etags.Any(e =>
                {
                    if (e.IsWeak)
                    {
                        // Weak etag
                        if (c.Etag.Contains(e.Tag))
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (e.Tag == c.Etag)
                        {
                            return true;
                        }
                    }

                    return false;
                });
            });
            return await ContinueExecution(concatenatedEtags, unmodifiedSince, representationId, checkDateCallback, checkEtagCorrectCallback);
        }

        public async Task<bool> CheckRepresentationHasChangedAsync(
            Controller controller,
            string representationId)
        {
            if (controller == null)
            {
                throw new ArgumentNullException(nameof(controller));
            }

            if (string.IsNullOrWhiteSpace(representationId))
            {
                throw new ArgumentNullException(nameof(representationId));
            }

            // Check the http request contains the header "If-None-Match"
            var concatenatedEtags = controller.GetIfNoneMatch();
            var modifiedSince = controller.GetModifiedSince();
            var checkDateCallback = new Func<DateTime, ConcurrentObject, bool>((d, c) =>
            {
                return c.DateTime > d;
            });
            var checkEtagCorrectCallback = new Func<ConcurrentObject, List<EntityTagHeaderValue>, bool>((c, etags) =>
            {
                return etags.All(etag =>
                {
                    if (etag.IsWeak)
                    {
                        // Weak etag
                        if (c.Etag.Contains(etag.Tag))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (etag.Tag == c.Etag)
                        {
                            return false;
                        }
                    }

                    return true;
                });
            });
            return await ContinueExecution(concatenatedEtags, modifiedSince, representationId, checkDateCallback, checkEtagCorrectCallback);
        }

        private async Task<bool> ContinueExecution(
            string concatenatedEtags,
            string modifiedDate,
            string representationId,
            Func<DateTime, ConcurrentObject, bool> checkDateCallback,
            Func<ConcurrentObject, List<EntityTagHeaderValue>, bool> checkEtagCorrectCallback)
        {
            if (string.IsNullOrWhiteSpace(concatenatedEtags) &&
                string.IsNullOrWhiteSpace(modifiedDate))
            {
                return true;
            }
            
            // Check a representation exists
            var lastRepresentation = await _concurrencyManager.TryGetRepresentationAsync(representationId);
            if (lastRepresentation == null)
            {
                throw new ArgumentNullException($"the representation {representationId} doesn't exist");
            }

            if (string.IsNullOrWhiteSpace(concatenatedEtags) || concatenatedEtags == "*")
            {
                // Process the date
                DateTime dateTime;
                if (!DateTime.TryParse(modifiedDate, out dateTime))
                {
                    return true;
                }

                return checkDateCallback(dateTime, lastRepresentation);
            }
            else
            {            
                // Check etags are correct
                var etagsStr = concatenatedEtags.Split(',');
                var etags = new List<EntityTagHeaderValue>();
                foreach (var etagStr in etagsStr)
                {
                    EntityTagHeaderValue et = null;
                    if (EntityTagHeaderValue.TryParse(etagStr, out et))
                    {
                        etags.Add(et);
                    }
                }

                if (checkEtagCorrectCallback(lastRepresentation, etags))
                {
                    return true;
                }
            }

            return false;
        }

        private void SetHeaders(Controller controller, ConcurrentObject concurrentObject)
        {
            controller.SetEtag(concurrentObject.Etag);
            controller.SetLastModifiedDate(concurrentObject.DateTime.ToUniversalTime().ToString("R"));
        }
    }
}