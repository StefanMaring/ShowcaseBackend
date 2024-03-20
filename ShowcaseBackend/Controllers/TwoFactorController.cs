using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QRCoder;

namespace ShowcaseBackend.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class TwoFactorController : ControllerBase {
        private readonly UserManager<IdentityUser> _userManager;

        public TwoFactorController(UserManager<IdentityUser> userManager) {
            _userManager = userManager;
        }
    }
}
