using System.ComponentModel.DataAnnotations;

namespace Talabat.APIs.DTO
{
    public class RegisterDto
    {
        [Required]
        public string DisplayName { get; set; } = null!;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        public string Phone { get; set; } = null!;

        [Required]
        [RegularExpression("(?=^.{6,10}$)(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%^&amp;*()_+}{&quot;:;'?/&gt;.&lt;,])(?!.*\\s).*$",
            ErrorMessage = "Password must have 1 Uppercase, 31 Lowercase, 1 Number, 1 non alphanumeric and at least 6 characters")]
        public string Password { get; set; } = null!;

    }
}
