using CSRFAttackInAspNetCoreMVC.Model;
using Microsoft.EntityFrameworkCore;

namespace CSRFAttackInAspNetCoreMVC.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<BlogPost> BlogPosts { get; set; }
    }
}
