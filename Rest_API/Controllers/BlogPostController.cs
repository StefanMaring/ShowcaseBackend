using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rest_API.Data;
using Rest_API.Models;

namespace Rest_API.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostController : ControllerBase {

        private readonly BlogContext _blogContext;

        [Authorize(Roles = UserRoles.Developer)]
        [HttpPost("CreatePost")]
        public async Task<IActionResult> Post([FromBody] BlogPost post) {
            if(ModelState.IsValid) {
                _blogContext.Posts.Add(post);
                await _blogContext.SaveChangesAsync();
                return Ok(new {message = "success"});
            } else {
                return BadRequest(new {message = "error"});
            }
        }

        [Authorize(Roles = UserRoles.Developer)]
        [HttpPost("DeletePost")]
        public async Task<IActionResult> Delete([FromBody] BlogPost post) {
            if(ModelState.IsValid) {
                _blogContext.Posts.Remove(post);
                await _blogContext.SaveChangesAsync();
                return Ok(new { message = "success" });
            } else {
                return BadRequest(new { message = "error" });
            }
        }
    }
}
