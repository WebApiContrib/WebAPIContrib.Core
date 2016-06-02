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

using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiContrib.Core.Concurrency.Extensions;

namespace WebApiContrib.Core.Concurrency
{
    public interface IRepresentationManager
    {
        Task AddOrUpdateRepresentationAsync(
            Controller controller, 
            string representationId);

        Task<bool> CheckRepresentationAsync(
            Controller controller,
            string representationId);
    }

    internal class RepresentationManager : IRepresentationManager
    {
        private readonly IConcurrencyManager _concurrencyManager;

        #region Constructor

        public RepresentationManager(IConcurrencyManager concurrencyManager)
        {
            _concurrencyManager = concurrencyManager;
        }

        #endregion

        #region Public methods

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
            controller.SetEtag(concurrentObject.Etag);
            controller.SetLastModifiedDate(concurrentObject.DateTime.ToUniversalTime().ToString("R"));
        }

        public async Task<bool> CheckRepresentationAsync(
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
            var lastRepresentation = await _concurrencyManager.TryGetRepresentationAsync(representationId);
            if (concatenatedEtags == null || lastRepresentation == null)
            {
                return false;
            }

            var etagsStr = concatenatedEtags.Split(',');
            var etags = new List<EntityTagHeaderValue>();
            foreach(var etagStr in etagsStr)
            {
                EntityTagHeaderValue et = null;
                if (EntityTagHeaderValue.TryParse(etagStr, out et))
                {
                    if (et.Tag == lastRepresentation.Etag)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        #endregion
    }
}
