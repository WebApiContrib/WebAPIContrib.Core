using Microsoft.Extensions.ObjectPool;
using Newtonsoft.Json;

namespace WebApiContrib.Core.Formatter.Bson
{
    public class BsonSerializerObjectPolicy : IPooledObjectPolicy<JsonSerializer>
    {
        private readonly JsonSerializerSettings _serializerSettings;

        public BsonSerializerObjectPolicy(JsonSerializerSettings serializerSettings)
        {
            _serializerSettings = serializerSettings;
        }

        public JsonSerializer Create() => JsonSerializer.Create(_serializerSettings);

        public bool Return(JsonSerializer serializer) => true;
    }
}
