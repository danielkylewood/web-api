using Newtonsoft.Json;

namespace WebApiTemplate.WebApi.Models.Hypermedia
{
    public class HypermediaLinks<T> where T : class
    {
        [JsonProperty("links")]
        public T Links { get; set; }

        public HypermediaLinks(T hypermediaResponse)
        {
            Links = hypermediaResponse;
        }
    }
}
