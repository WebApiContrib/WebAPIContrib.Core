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

using System.Threading.Tasks;

namespace WebApiContrib.Core.Concurrency.Storage
{
    public interface IStorage
    {
        ConcurrentObject TryGetValue(string key);

        Task<ConcurrentObject> TryGetValueAsync(string key);

        void Set(string key, ConcurrentObject value);

        Task SetAsync(string key, ConcurrentObject value);

        void Remove(string key);

        Task RemoveAsync(string key);
    }
}
