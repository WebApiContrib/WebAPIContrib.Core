#region copyright
// Copyright 2016 WebApiContrib
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion

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

        #region Constructor

        public ConcurrencyManager(
            ConcurrencyOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _options = options;
        }

        #endregion

        #region Public methods

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

        #endregion
    }
}
