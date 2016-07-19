using Newtonsoft.Json;

namespace WebApiContrib.Core.Formatter.Bson
{
    public static class BsonSerializerSettingsProvider
    {
        public static JsonSerializerSettings CreateSerializerSettings()
        {
            return new JsonSerializerSettings
            {
                MissingMemberHandling = MissingMemberHandling.Ignore,
                TypeNameHandling = TypeNameHandling.None
            };
        }
    }
}
