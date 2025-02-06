using System.ComponentModel.DataAnnotations;

namespace MadiffApi.Validators
{
    public class UserIdValidator : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult("User ID cannot be null");
            }

            var userId = value.ToString();

            if (string.IsNullOrWhiteSpace(userId) || !userId.StartsWith("User", StringComparison.OrdinalIgnoreCase))
            {
                return new ValidationResult("Invalid user ID format. Should start with 'User'");
            }

            return ValidationResult.Success;
        }
    }
}
