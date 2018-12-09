using System.Collections.Generic;
using Newtonsoft.Json;

namespace WebApiTemplate.WebApi.Models
{
    public class CreatedResponse<TDiscovery>
    {
        public string ResponseCode { get; set; }
        public IDictionary<string, string> ResponseData { get; set; }

        [JsonProperty("links")]
        public TDiscovery Links { get; set; }

        public CreatedResponse()
        {

        }

        public CreatedResponse(string responseCode, TDiscovery links)
        {
            ResponseData = new Dictionary<string, string>();
            ResponseCode = responseCode;
            Links = links;
        }

        public CreatedResponse(string responseCode, TDiscovery links, IDictionary<string, string> responseData)
        {
            ResponseCode = responseCode;
            ResponseData = responseData;
            Links = links;
        }
    }
}
