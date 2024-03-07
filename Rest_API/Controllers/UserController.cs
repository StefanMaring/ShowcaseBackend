using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rest_API.Data;
using Rest_API.Models;

namespace Rest_API.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase {

        private readonly BlogContext _blogContext;

        //[HttpPost(Name = "CreateUser")]
        //public async Task<IActionResult> Create([FromBody] User user) {
        //    if(ModelState.IsValid) {
        //        _blogContext.Add(user);
        //        await _blogContext.SaveChangesAsync();
        //        return Ok(new { message = "Je bent succesvol geregistreerd!" });
        //    } else {
        //        return BadRequest(new {message = "Er is een fout opgetreden"});
        //    }
        //}

        [HttpGet("test")]
        [Authorize]
        public IActionResult Get() {
            return Ok("Test");
        }

        //[HttpPost(Name = "LoginUser")]
        //public async Task<IActionResult> Post([FromBody] User user) {

        //}
    }
}
