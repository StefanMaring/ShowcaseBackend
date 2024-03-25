using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Rest_API.Models;
using ShowcaseBackend.Models;

namespace Rest_API.Data {
    public class BlogContext : IdentityDbContext<AppUser>
    {
        public BlogContext(DbContextOptions<BlogContext> options) : base(options) { 

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<BlogPost> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<AppUser> Users {  get; set; }
    }
}
