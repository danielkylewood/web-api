using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace WebApiTemplate.Tests.Integration.Helpers
{
    public static class SerializerHelper
    {
        public static string SerializeTo(object obj)
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                }
            };
            var json = JsonConvert.SerializeObject(obj, settings);
            return json;
        }

        public static T DeserializeFrom<T>(string json)
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                }
            };
            var obj = JsonConvert.DeserializeObject<T>(json, settings);
            return obj;
        }
    }
}