using System.ComponentModel.DataAnnotations;

namespace Venjix.Infrastructure.TagHelpers
{
    public class NotNullOrWhiteSpaceAttribute : ValidationAttribute
    {
        public NotNullOrWhiteSpaceAttribute() : base("Invalid Field")
        {
        }

        public NotNullOrWhiteSpaceAttribute(string Message) : base(Message)
        {
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (!string.IsNullOrWhiteSpace(value?.ToString())) return ValidationResult.Success;
            return new ValidationResult("Value cannot be empty or white space.");
        }
    }
}