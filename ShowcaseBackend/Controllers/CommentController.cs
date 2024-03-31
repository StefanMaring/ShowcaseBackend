using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
                    CommentUser = FormValidation.StripHTML(comment.CommentUser),
                    CommentText = FormValidation.StripHTML(comment.CommentText),
                    BlogPostID = comment.BlogPostID,
                };
                _blogContext.Comments.Add(newComment);
                await _blogContext.SaveChangesAsync();

                await _blogHub.SendNewComment();

                return Ok(new Response { Status = "Success", Message = "Comment succesvol gepost" });
            }
            else {
                return BadRequest(new Response { Status = "Error", Message = "Er is een fout opgetreden, comment niet gepost" });
            }
        }

        [HttpGet("GetCommentsByBlogID/{blogID}")]
        public async Task<IActionResult> GetCommentsByBlogID(string blogID) {
            var commentsPerPost = _blogContext.Comments
                .Where(c => c.BlogPostID == blogID)
                .Select(c => new { c.CommentId, c.CommentUser, c.CommentText })
                .ToListAsync();

            if (commentsPerPost != null) {
                return Ok(commentsPerPost);
            }
            else {
                return NotFound(new Response { Status = "Not Found", Message = "Er zijn geen comments gevonden" });
            }
        }

        [HttpPost("DeleteComment")]
        public async Task<IActionResult> Delete([FromBody] DeleteCommentModel comment) {
            var commentToDelete = _blogContext.Comments.FirstOrDefault(c => c.CommentId == comment.CommentId);

            if (commentToDelete != null) {
                _blogContext.Comments.Remove(commentToDelete);
                await _blogContext.SaveChangesAsync();

                await _blogHub.OnDeleteComment(comment.CommentId);

                return Ok(new Response { Status = "Success", Message = "Comment succesvol verwijderd" });
            }
            else {
                return NotFound(new Response { Status = "Not Found", Message = "De Comment is niet verwijderd" });
            }
        }
    }
}
