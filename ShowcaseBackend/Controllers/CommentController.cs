using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Rest_API.Data;
using Rest_API.Hubs;
using Rest_API.Models;
using Rest_API_ClassLibrary;
using ShowcaseBackend.Models;

namespace ShowcaseBackend.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase {

        private readonly ILogger _logger;
        private readonly BlogContext _blogContext;
        private readonly BlogHub _blogHub;

        public CommentController(ILogger<BlogContext> logger, BlogContext blogContext, BlogHub blogHub) {
            _logger = logger;
            _blogContext = blogContext;
            _blogHub = blogHub;
        }

        [Authorize]
        [HttpPost("CreateComment")]
        public async Task<IActionResult> Post([FromBody] CreateCommentModel comment) {
            if (ModelState.IsValid) {
                var newComment = new Comment {
                    CommentId = Guid.NewGuid().ToString(),
                    CommentUser = comment.CommentUser,
                    CommentDate = comment.CommentDate,
                    CommentText = comment.CommentText,
                    BlogPostID = comment.BlogPostID,
                };
                _blogContext.Comments.Add(newComment);
                await _blogContext.SaveChangesAsync();

                //await _blogHub.SendNewPost();

                return Ok(new Response { Status = "Success", Message = "Comment succesvol gepost" });
            }
            else {
                return BadRequest(new Response { Status = "Error", Message = "Er is een fout opgetreden, comment niet gepost" });
            }
        }
    }
}
