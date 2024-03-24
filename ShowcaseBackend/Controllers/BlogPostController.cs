using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rest_API.Data;
using Rest_API.Hubs;
using Rest_API.Models;
using Rest_API_ClassLibrary;

namespace Rest_API.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostController : ControllerBase {

        private readonly ILogger _logger;
        private readonly BlogContext _blogContext;
        private readonly BlogHub _blogHub;

        public BlogPostController(ILogger<BlogContext> logger, BlogContext blogContext, BlogHub blogHub)
        {
            _logger = logger;
            _blogContext = blogContext;
            _blogHub = blogHub;
        }

        [Authorize(Roles = UserRoles.Developer)]
        [HttpPost("CreatePost")]
        public async Task<IActionResult> Post([FromBody] CreatePostModel post) {
            if (ModelState.IsValid) { 
                var blogPost = new BlogPost{
                    Id = Guid.NewGuid().ToString(),
                    PostAuthor = post.PostAuthor,
                    PostDate = post.PostDate,
                    PostTitle = post.PostTitle,
                    PostText = FormValidation.SanitizeHtml(post.PostText),
                };
                _blogContext.Posts.Add(blogPost);
                await _blogContext.SaveChangesAsync();

                await _blogHub.SendNewPost();

                return Ok(new Response {Status = "Success", Message = "Post succesvol aangemaakt"});
            } else {
                return BadRequest(new Response { Status = "Error", Message = "Er is een fout opgetreden, post niet aangemaakt" });
            }
        }

        [Authorize(Roles = UserRoles.Developer)]
        [HttpPost("DeletePost")]
        public async Task<IActionResult> Delete([FromBody] DeletePostModel post) {
            var postToDelete = _blogContext.Posts.FirstOrDefault(p => p.Id == post.PostID);

            if(postToDelete != null)
            {
                _blogContext.Posts.Remove(postToDelete);
                await _blogContext.SaveChangesAsync();

                await _blogHub.OnDeletePost(post.PostID);

                return Ok(new Response {Status = "Success", Message = "Post succesvol verwijderd"});
            } else
            {
                return NotFound(new Response {Status = "Not Found", Message = "De post is niet verwijderd"});
            }
        }

        [HttpGet("GetAllPosts")]
        public async Task<IActionResult> GetAllPosts()
        {
            var blogPosts = _blogContext.Posts
                .Select(p => new {p.Id, p.PostTitle})
                .ToListAsync();

            if (blogPosts != null)
            {
                return Ok(blogPosts);
            } else
            {
                return NotFound(new Response { Status = "Not Found", Message = "Er zijn geen artikelen beschikbaar"});
            }
        }

        [HttpGet("GetPostByID")]
        public async Task<IActionResult> GetPostByID([FromQuery] string postID)
        {
            var post = _blogContext.Posts.Where(p => p.Id == postID).FirstOrDefault();

            if (post != null)
            {
                return Ok(post);
            }
            else
            {
                return NotFound(new Response {Status = "Not Found", Message = "Het artikel is niet gevonden" });
            }
        }
    }
}
