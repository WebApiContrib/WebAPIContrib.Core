using System;
using System.Threading.Tasks;

namespace WebApiContrib.Core.Concurrency
{
    public interface IConcurrencyManager
    {
        ConcurrentObject TryUpdateRepresentation(string representationId);

        Task<ConcurrentObject> TryUpdateRepresentationAsync(string representationId);

        Task<bool> IsRepresentationDifferentAsync(string representationId, string etag);

        ConcurrentObject TryGetRepresentation(string representationId);

        Task<ConcurrentObject> TryGetRepresentationAsync(string representationId);

        void Remove(string representationId);

        Task RemoveAsync(string representationId);
    }

    internal class ConcurrencyManager : IConcurrencyManager
    {
        private readonly ConcurrencyOptions _options;

        public ConcurrencyManager(
            ConcurrencyOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _options = options;
        }

        public ConcurrentObject TryUpdateRepresentation(string representationId)
        {
            return TryUpdateRepresentationAsync(representationId).Result;
        }

        public async Task<ConcurrentObject> TryUpdateRepresentationAsync(string representationId)
        {
            if (string.IsNullOrWhiteSpace(representationId))
            {
                throw new ArgumentNullException(nameof(representationId));
            }

            var concurrentObject = new ConcurrentObject
            {
                Etag = "\""+ Guid.NewGuid().ToString() + "\"",
                DateTime = DateTime.UtcNow
            };
            await _options.Storage.SetAsync(representationId, concurrentObject);
            return concurrentObject;
        }

        public async Task<bool> IsRepresentationDifferentAsync(string representationId, string etag)
        {
            var representation = await TryGetRepresentationAsync(representationId);
            if (representation == null)
            {
                return false;
            }

            return representation.Etag.ToString() != etag;
        }

        public ConcurrentObject TryGetRepresentation(string name)
        {
            return TryGetRepresentationAsync(name).Result;
        }

        public async Task<ConcurrentObject> TryGetRepresentationAsync(string representationId)
        {
            if (string.IsNullOrWhiteSpace(representationId))
            {
                throw new ArgumentNullException(nameof(representationId));
            }

            var value = await _options.Storage.TryGetValueAsync(representationId);
            if (value == null || !(value is ConcurrentObject))
            {
                return null;
            }

            return value as ConcurrentObject;
        }

        public void Remove(string name)
        {
            RemoveAsync(name).Wait();
        }

        public async Task RemoveAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            await _options.Storage.RemoveAsync(name);
        }
    }
}
