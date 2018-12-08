using System;
using System.Linq;
using System.Reflection;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApiTemplate.WebApi.Models;
using BadRequestObjectResult = WebApiTemplate.WebApi.Models.BadRequestObjectResult;

namespace WebApiTemplate.WebApi.Filters
{
    public class ValidateRequestModelFilter : IActionFilter
    {
        private readonly IValidatorFactory _validatorFactory;
        private const string _requestModelInvalid = "request_invalid";

        public ValidateRequestModelFilter(IValidatorFactory validatorFactory)
        {
            _validatorFactory = validatorFactory ?? throw new ArgumentNullException(nameof(validatorFactory));
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            foreach (var parameterDescriptor in context.ActionDescriptor.Parameters)
            {
                var parameter = (ControllerParameterDescriptor)parameterDescriptor;
                var typeInfo = parameter.ParameterType.GetTypeInfo();

                if (!typeInfo.IsClass || !context.ActionArguments.TryGetValue(parameter.Name, out var parameterValue))
                    continue;

                switch (parameterValue)
                {
                    case null when !parameter.ParameterInfo.IsOptional:
                    {
                        var traceIdentifier = context.HttpContext.TraceIdentifier;
                        var validationError = new ValidationError(traceIdentifier, _requestModelInvalid, new[] {"request_body_required"});
                        context.Result = new Microsoft.AspNetCore.Mvc.BadRequestObjectResult(validationError);
                        return;
                    }
                    case null:
                        return;
                }

                var validator = _validatorFactory.GetValidator(parameter.ParameterType);
                var validationResult = validator?.Validate(parameterValue);

                if (validationResult == null || validationResult.IsValid)
                    continue;

                var errorCodes = validationResult.Errors.Select(e => e.ErrorCode).OrderBy(x => x).ToList();
                var errorResponse = new ValidationError(context.HttpContext.TraceIdentifier, _requestModelInvalid, errorCodes);
                context.Result = new BadRequestObjectResult(errorResponse);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
