using Microsoft.AspNetCore.Mvc;
using WebApiTemplate.WebApi.Models;
using WebApiTemplate.WebApi.Models.Hypermedia;

namespace WebApiTemplate.WebApi.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {
        [HttpGet("", Name = "Home")]
        public IActionResult Get()
        {
            var model = new HypermediaLinks<ApiDiscovery>(HypermediaLinkBuilder.ForServiceDiscovery(Url));
            return Ok(model);
        }
    }
}