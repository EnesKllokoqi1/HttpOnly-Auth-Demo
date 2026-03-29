using System.ComponentModel.DataAnnotations;

namespace CookieAPI.DTOs
{
    public class UserRegisterDTO
    {
        [Required(ErrorMessage = "First name is required")]
        [MaxLength(50, ErrorMessage = "First name cannot exceed 50 characters")]
        [MinLength(2, ErrorMessage = "First name must be at least 2 characters")]
        [RegularExpression(
      @"^[\p{L}]+([\p{L}\s'\-]*[\p{L}])$",
      ErrorMessage = "First name can only contain letters, spaces, hyphens, and apostrophes"
      )]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required")]
        [MaxLength(50, ErrorMessage = "Last name cannot exceed 50 characters")]
        [MinLength(2, ErrorMessage = "Last name must be at least 2 characters")]
        [RegularExpression(
        @"^[\p{L}]+([\p{L}\s'\-]*[\p{L}])$",
        ErrorMessage = "Last name can only contain letters, spaces, hyphens, and apostrophes"
        )]
        public string LastName { get; set; } = string.Empty;


        [Required(ErrorMessage = "Gender is required")]
        public char Gender { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
        [MaxLength(100, ErrorMessage = "Password cannot exceed 100 characters")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$",
            ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress(ErrorMessage = "Invalid email address format")]
        [MaxLength(254, ErrorMessage = "Email address cannot exceed 254 characters")]
        public string EmailAddress { get; set; } = string.Empty;


        [Range(1, 120, ErrorMessage = "Please enter a valid age")]
        [Required(ErrorMessage = "Age is required")]
        public int Age { get; set; }

    }
}
