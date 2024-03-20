using Rest_API.Models;

namespace ShowcaseBackend.Models {
    public class CreateCommentModel {
        public string CommentUser { get; set; }
        public string CommentText { get; set; }
        public string BlogPostID { get; set; }
    }
}
