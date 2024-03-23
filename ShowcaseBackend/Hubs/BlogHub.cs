using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.SignalR;
using Rest_API.Data;
using Rest_API.Models;

namespace Rest_API.Hubs {

    public class BlogHub : Hub
    {
        private readonly BlogContext _blogContext;
        protected IHubContext<BlogHub> _context;

        public BlogHub(BlogContext blogContext, IHubContext<BlogHub> context)
        {
            _blogContext = blogContext;
            _context = context;
        }

        public async Task SendNewPost() {
            var newPostData = _blogContext.Posts.Select(p => new {p.Id, p.PostTitle}).ToList().Last();

            if (newPostData != null) {
                await _context.Clients.All.SendAsync("ReceiveNewPost", new { post = newPostData });
            }
        }

        public async Task OnDeletePost(string postID)
        {
            await _context.Clients.All.SendAsync("ReceiveDeletePost", new { postToHide = postID });
        }

        public async Task SendNewComment() {
            var newCommentData = _blogContext.Comments.Select(c => new { c.CommentId, c.CommentUser, c.CommentText }).ToList().Last();

            if (newCommentData != null) {
                await _context.Clients.All.SendAsync("ReceiveNewComment", new { comment = newCommentData });
            }
        }
    }
}
