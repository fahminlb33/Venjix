using System;
using System.ComponentModel.DataAnnotations;

namespace Venjix.Infrastructure.Helpers
{
    [AttributeUsage(AttributeTargets.Property)]
    public class RequiredIfAttribute : RequiredAttribute
    {
        public string OtherProperty { get; set; }
        public object OtherValue { get; set; }

        public RequiredIfAttribute(string otherProperty, object otherValue)
        {
            OtherProperty = otherProperty;
            OtherValue = otherValue;
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            if (string.IsNullOrWhiteSpace(OtherProperty)) return ValidationResult.Success;

            object instance = context.ObjectInstance;
            Type type = instance.GetType();

            var prop = type.GetProperty(OtherProperty).GetValue(instance);
            return prop.Equals(OtherValue) && value == null ? new ValidationResult(ErrorMessage) : ValidationResult.Success;
        }
    }
}
