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
