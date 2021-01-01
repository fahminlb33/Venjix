using System;
using System.ComponentModel.DataAnnotations;

namespace Venjix.Infrastructure.TagHelpers
{
    public class NotEqualAttribute : ValidationAttribute
    {
        public string Value { get; private set; }
        public bool CaseInsensitive { get; private set; }

        public NotEqualAttribute(string value, bool caseInsensitive = true)
        {
            Value = value;
            CaseInsensitive = caseInsensitive;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(value?.ToString() ?? "")) return ValidationResult.Success;
            if (value.ToString().Equals(Value, CaseInsensitive ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal))
            {
                return new ValidationResult($"Value cannot be {Value}, use another.");
            }

            return ValidationResult.Success;
        }
    }
}
