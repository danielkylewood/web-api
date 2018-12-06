using System;
using System.Linq;
using FluentValidation;
using WebApiTemplate.Domain.Models;
using WebApiTemplate.WebApi.Models;

namespace WebApiTemplate.WebApi.Validators
{
    public class CustomerValidator : AbstractValidator<CustomerRequest>
    {
        public CustomerValidator()
        {
            RuleFor(x => x.FirstName)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull()
                    .WithErrorCode(ErrorCodes.FirstNameRequired)
                .NotEmpty()
                    .WithErrorCode(ErrorCodes.FirstNameRequired)
                .MaximumLength(70)
                    .WithErrorCode(ErrorCodes.FirstNameLengthExceeded)
                .Must(ContainsOnlyLettersOrDigitsAndSpaces)
                    .WithErrorCode(ErrorCodes.FirstNameInvalid);

            RuleFor(x => x.Surname)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull()
                    .WithErrorCode(ErrorCodes.SurnameRequired)
                .NotEmpty()
                    .WithErrorCode(ErrorCodes.SurnameRequired)
                .MaximumLength(70)
                    .WithErrorCode(ErrorCodes.SurnameLengthExceeded)
                .Must(ContainsOnlyLettersOrDigitsAndSpaces)
                    .WithErrorCode(ErrorCodes.SurnameInvalid);

            RuleFor(x => x.Status)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull()
                    .WithErrorCode(ErrorCodes.StatusRequired)
                .NotEmpty()
                    .WithErrorCode(ErrorCodes.StatusRequired)
                .Must(IsValidStatus)
                    .WithErrorCode(ErrorCodes.StatusInvalid);

            RuleFor(x => x.ExternalCustomerReference)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull()
                    .WithErrorCode(ErrorCodes.ExternalCustomerReferenceRequired)
                .NotEmpty()
                    .WithErrorCode(ErrorCodes.ExternalCustomerReferenceRequired)
                .Must(IsValidExternalCustomerReference)
                    .WithErrorCode(ErrorCodes.ExternalCustomerReferenceInvalid);
        }

        private static bool ContainsOnlyLettersOrDigitsAndSpaces(string str)
        {
            return str.All(x => char.IsLetterOrDigit(x) || char.IsWhiteSpace(x));
        }

        private static bool IsValidStatus(string status)
        {
            var canParse = Enum.TryParse(typeof(Status), status, true, out var _);
            return canParse;
        }

        private static bool IsValidExternalCustomerReference(string status)
        {
            var canParse = Enum.TryParse(typeof(Status), status, true, out var _);
            return canParse;
        }
    }
}