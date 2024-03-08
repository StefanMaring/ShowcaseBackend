using Microsoft.AspNetCore.Identity;

namespace Rest_API.Models {
    public class User : IdentityUser {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
