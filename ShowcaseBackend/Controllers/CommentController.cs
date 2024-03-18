using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShowcaseBackend.Models;

namespace ShowcaseBackend.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase {

        [Authorize]
        [HttpPost("CreateComment")]
        public async Task<IActionResult> Post([FromBody] CreateCommentModel comment) {
            return Ok(comment);
        }
    }
}
