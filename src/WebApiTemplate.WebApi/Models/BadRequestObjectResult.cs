using Microsoft.AspNetCore.Mvc;

namespace WebApiTemplate.WebApi.Models
{
    public class BadRequestObjectResult : ObjectResult
    {
        public BadRequestObjectResult(object value) : base(value)
        {
            StatusCode = 400;
        }
    }
}
