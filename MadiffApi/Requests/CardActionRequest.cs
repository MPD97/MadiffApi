using MadiffApi.Validators;
using System.ComponentModel.DataAnnotations;

namespace MadiffApi.Requests
{
    public class CardActionRequest
    {
        private const string CardNumberPattern = @"^Card\d+$";

        [Required(ErrorMessage = "User ID is required")]
        [MinLength(3, ErrorMessage = "User ID must be at least 3 characters long")]
        [MaxLength(50, ErrorMessage = "User ID cannot exceed 50 characters")]
        [UserIdValidator]
        public string UserId { get; set; }

        [Required(ErrorMessage = "Card number is required")]
        [RegularExpression(CardNumberPattern, 
            ErrorMessage = "Invalid card number format. Should be in format 'CardXXX'",
            MatchTimeoutInMilliseconds = 100)]
        public string CardNumber { get; set; }
    }
}
