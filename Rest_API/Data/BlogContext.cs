using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Rest_API.Models;

namespace Rest_API.Data {
    public class BlogContext : IdentityDbContext<User> {
        public BlogContext(DbContextOptions<BlogContext> options) : base(options) { 

        }
    }
}
