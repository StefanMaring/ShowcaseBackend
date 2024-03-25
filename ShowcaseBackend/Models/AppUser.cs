using Microsoft.AspNetCore.Identity;

namespace ShowcaseBackend.Models {
    public class AppUser : IdentityUser {
        public string TwoFactorSecret { get; set; }
    }
}
