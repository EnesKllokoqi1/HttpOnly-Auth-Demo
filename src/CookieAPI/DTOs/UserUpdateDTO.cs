using System.ComponentModel.DataAnnotations;

namespace CookieAPI.DTOs
{
    public class UserUpdateDTO
    {
        [MaxLength(50, ErrorMessage = "First name cannot exceed 50 characters")]
        [MinLength(2, ErrorMessage = "First name must be at least 2 characters")]
        [RegularExpression(
            @"^[\p{L}]+([\p{L}\s'\-]*[\p{L}])$",
            ErrorMessage = "First name can only contain letters, spaces, hyphens, and apostrophes")]
        public string FirstName { get; set; }

        [MaxLength(50, ErrorMessage = "Last name cannot exceed 50 characters")]
        [MinLength(2, ErrorMessage = "Last name must be at least 2 characters")]
        [RegularExpression(
            @"^[\p{L}]+([\p{L}\s'\-]*[\p{L}])$",
            ErrorMessage = "Last name can only contain letters, spaces, hyphens, and apostrophes")]
        public string LastName { get; set; }

        public char Gender { get; set; }

        [Range(1, 120, ErrorMessage = "Please enter a valid age")]
        public int Age { get; set; }
    }
}
