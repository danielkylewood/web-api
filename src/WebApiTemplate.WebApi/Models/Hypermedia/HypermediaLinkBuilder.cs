using System;
using Microsoft.AspNetCore.Mvc;

namespace WebApiTemplate.WebApi.Models.Hypermedia
{
    public class HypermediaLinkBuilder
    {
        public static ApiDiscovery ForServiceDiscovery(IUrlHelper url)
        {
            return new ApiDiscovery(
                GetLinkForRootDiscovery(url),
                GetLinkForCustomerCreate(url),
                GetLinkForCustomerUpdate(url),
                GetLinkForCustomerGet(url)
            );
        }

        public static CustomerDiscovery ForCustomerDiscovery(IUrlHelper url, string customerReference)
        {
            return new CustomerDiscovery(
                GetLinkForCustomerDiscovery(url, customerReference),
                GetLinkForCustomerUpdate(url, customerReference)
            );
        }
        
        private static Link GetLinkForRootDiscovery(IUrlHelper url)
        {
            var link = url.Link("Home", new { });
            return ConvertLink(link);
        }

        private static Link GetLinkForCustomerDiscovery(IUrlHelper url, string customerReference)
        {
            var link = url.Link("Home", new { });
            return ConvertLink(link, $"customers/{customerReference}");
        }

        private static Link GetLinkForCustomerCreate(IUrlHelper url)
        {
            var link = url.Link("Home", new { });
            return ConvertLink(link, "customers");
        }

        private static Link GetLinkForCustomerGet(IUrlHelper url)
        {
            var link = url.Link("Home", new { });
            return ConvertLink(link, "customers/{customerReference}");
        }

        private static Link GetLinkForCustomerUpdate(IUrlHelper url)
        {
            var link = url.Link("Home", new { });
            return ConvertLink(link, "customers/{customerReference}");
        }

        private static Link GetLinkForCustomerUpdate(IUrlHelper url, string customerReference)
        {
            var link = url.Link("Home", new { });
            return ConvertLink(link, $"customers/{customerReference}");
        }

        private static Link ConvertLink(string link, string linkSuffix = "")
        {
            var uriBuilder = LocalhostLink(link);
            return new Link { Href = uriBuilder.Uri + linkSuffix};
        }

        private static UriBuilder LocalhostLink(string link)
        {
            var builder = new UriBuilder(new Uri(link))
            {
                Scheme = Uri.UriSchemeHttp
            };
            return builder;
        }
    }
}
