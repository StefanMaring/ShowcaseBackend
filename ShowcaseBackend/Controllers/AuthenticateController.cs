using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Rest_API.Data;
using Rest_API.Models;
using Rest_API_ClassLibrary;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Rest_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthenticateController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if(!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByNameAsync(model.Username);

            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var token = GetToken(authClaims);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }

            return Unauthorized(new Response {Status = "Error", Message = "Login mislukt, er is een fout opgetreden"});
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var userExists = await _userManager.FindByNameAsync(model.Username);

            if (userExists != null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Gebruiker bestaat al!" });
            }

            IdentityUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Er is een fout opgetreden, gebruiker niet geregistreerd!" });
            }
            else
            {
                return Ok(new Response { Status = "Success", Message = "Gebruiker succesvol geregistreerd!" });
            }
        }

        [HttpPost]
        [Route("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Gebruiker bestaat al!" });

            IdentityUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Er is een fout opgetreden, gebruiker niet geregistreerd!" });

            if (!await _roleManager.RoleExistsAsync(UserRoles.Developer))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Developer));

            if (!await _roleManager.RoleExistsAsync(UserRoles.Lezer))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Lezer));

            if (await _roleManager.RoleExistsAsync(UserRoles.Developer))
            {
                await _userManager.AddToRoleAsync(user, UserRoles.Developer);
            }

            if (await _roleManager.RoleExistsAsync(UserRoles.Developer))
            {
                await _userManager.AddToRoleAsync(user, UserRoles.Lezer);
            }

            return Ok(new Response { Status = "Success", Message = "Gebruiker succesvol geregistreerd!" });
        }

        [Authorize(Roles = UserRoles.Developer)]
        [HttpGet("GetUserID/{username}")]
        public async Task<IActionResult> GetUserID(string username) {
            var user = await _userManager.FindByNameAsync(username);

            if (user != null) {
                return (Ok(user.Id));
            }
            else if (user == null) {
                return NotFound();
            } else {
                return Unauthorized();
            }
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }
    }
}
