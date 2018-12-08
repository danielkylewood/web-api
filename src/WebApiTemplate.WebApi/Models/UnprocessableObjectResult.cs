using Microsoft.AspNetCore.Mvc;

namespace WebApiTemplate.WebApi.Models
{
    public class UnprocessableObjectResult : ObjectResult
    {
        public UnprocessableObjectResult(object value) : base(value)
        {
            StatusCode = 422;
        }
    }
}
