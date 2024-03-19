using Rest_API.Models;

namespace ShowcaseBackend.Models {
    public class Comment {
        public string CommentId { get; set; }
        public string CommentUser { get; set; }
        public string CommentDate { get; set; }
        public string CommentText { get; set; }
        public string BlogPostID { get; set; }
        public BlogPost BlogPost { get; set; }
    }
}
