using System.Collections.Generic;

namespace WebApiTemplate.WebApi.Models
{
    public class ValidationError
    {
        public string RequestId { get; set; }
        public string ErrorType { get; set; }
        public IEnumerable<string> ErrorCodes { get; set; }

        public ValidationError()
        {
        }

        public ValidationError(string requestId, string errorType, IEnumerable<string> errorCodes)
        {
            RequestId = requestId;
            ErrorType = errorType;
            ErrorCodes = errorCodes;
        }
    }
}
