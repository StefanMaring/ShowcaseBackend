using System.ComponentModel.DataAnnotations;

namespace Rest_API.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "User Name is required")]
        [MaxLength(50)]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MaxLength(50)]
        [MinLength(12)]
        public string? Password { get; set; }
    }
}
