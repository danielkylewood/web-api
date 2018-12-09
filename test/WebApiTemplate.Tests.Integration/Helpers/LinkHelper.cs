using System;
using System.Web;
using NUnit.Framework;
using WebApiTemplate.WebApi.Models;
using WebApiTemplate.WebApi.Models.Hypermedia;

namespace WebApiTemplate.Tests.Integration.Helpers
{
    public static class LinkHelper
    {
        public static void AssertLink(Link link, string href)
        {
            Assert.IsNotNull(link);
            Assert.IsNotEmpty(link.Href);
            var linkUri = HttpUtility.UrlDecode(new Uri(link.Href).AbsolutePath);
            Assert.That(href, Is.EqualTo(linkUri).IgnoreCase);
        }
    }
}
