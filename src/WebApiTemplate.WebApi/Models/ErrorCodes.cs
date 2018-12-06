namespace WebApiTemplate.WebApi.Models
{
    public static class ErrorCodes
    {
        public const string ExternalCustomerReferenceRequired = "external_customer_reference_required";
        public const string ExternalCustomerReferenceInvalid = "external_customer_reference_invalid";
        
        public const string FirstNameRequired = "first_name_required";
        public const string FirstNameInvalid = "first_name_invalid";
        public const string FirstNameLengthExceeded = "first_name_length_exceeded";

        public const string SurnameRequired = "surname_required";
        public const string SurnameInvalid = "surname_invalid";
        public const string SurnameLengthExceeded = "surname_length_exceeded";

        public const string StatusRequired = "status_required";
        public const string StatusInvalid = "status_invalid"; 
    }
}
