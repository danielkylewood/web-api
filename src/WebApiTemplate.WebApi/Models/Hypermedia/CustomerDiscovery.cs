using Newtonsoft.Json;

namespace WebApiTemplate.WebApi.Models.Hypermedia
{
    public class CustomerDiscovery
    {
        public Link Self { get; set; }

        [JsonProperty("customer-update")]
        public Link CustomerUpdate { get; set; }

        public CustomerDiscovery()
        {
        }

        public CustomerDiscovery(
            Link self,
            Link customerUpdate
        )
        {
            Self = self;
            CustomerUpdate = customerUpdate;
        }
    }
}
