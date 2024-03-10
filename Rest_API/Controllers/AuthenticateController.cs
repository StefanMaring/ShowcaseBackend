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
            //Find the user by name
            var user = await _userManager.FindByNameAsync(model.Username);

            //Check if the user is not null and if the password is correct
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                //Get the roles of the user
                var userRoles = await _userManager.GetRolesAsync(user);

                //Set the claims for the user
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                //Add the roles to the claims
                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var token = GetToken(authClaims);

                //Return the JWT token
                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }

            //If the authentication failed, return unauthorized
            return Unauthorized();
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            //Check if a user by the same name already exists
            var userExists = await _userManager.FindByNameAsync(model.Username);

            //If user is found, return 500 with user already exists message
            if (userExists != null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });
            }

            //Make new user
            IdentityUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };

            //Create the user
            var result = await _userManager.CreateAsync(user, model.Password);

            //If user creation fails, return 500 with message
            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });
            }
            else
            {
                //If user creation works, return 200 OK
                return Ok(new Response { Status = "Success", Message = "User created successfully!" });
            }
        }

        [HttpPost]
        [Route("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

            IdentityUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

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

            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
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
