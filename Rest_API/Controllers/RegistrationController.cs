using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Rest_API.Data;
using Rest_API.Models;

namespace Rest_API.Controllers {

    [EnableCors]
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase {
        private readonly BlogContext _blogContext;

        [HttpPost(Name = "CreateUser")]
        public async Task<IActionResult> Post([FromBody] User user) {
            if(ModelState.IsValid) {
                _blogContext.AddAsync(user);
                _blogContext.SaveChangesAsync();

                return Ok(new { message = "success" });
            } else {
                return BadRequest(new { message = "error" });
            }
        }
    }
}
