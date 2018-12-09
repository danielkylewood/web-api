using Newtonsoft.Json;

namespace WebApiTemplate.WebApi.Models.Hypermedia
{
    public class ApiDiscovery
    {
        public Link Self { get; set; }

        [JsonProperty("customer-create")]
        public Link CustomerCreate { get; set; }

        [JsonProperty("customer-update")]
        public Link CustomerUpdate { get; set; }

        [JsonProperty("customer-get")]
        public Link CustomerGet { get; set; }

        public ApiDiscovery(
            Link self,
            Link customerCreate,
            Link customerUpdate,
            Link customerGet
        )
        {
            Self = self;
            CustomerCreate = customerCreate;
            CustomerUpdate = customerUpdate;
            CustomerGet = customerGet;
        }
    }
}
