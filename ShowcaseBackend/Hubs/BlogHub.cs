using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.SignalR;
using Rest_API.Data;
using Rest_API.Models;

namespace Rest_API.Hubs {

    public class BlogHub : Hub
    {
        private readonly BlogContext _blogContext;

        public BlogHub(BlogContext blogContext) {
            _blogContext = blogContext;
        }

        public async Task SendNewPost() {
            var newPostData = _blogContext.Posts
                .FirstOrDefault();

            if (newPostData != null) {
                await Clients.All.SendAsync("ReceiveNewPost", new { newPost = newPostData });
            }
        }
    }
}
