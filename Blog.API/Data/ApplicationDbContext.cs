using Blog.API.Models.Domain;
using BLOG.API.Data;
using Microsoft.EntityFrameworkCore;

namespace Blog.API.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions <ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<BlogImage> BlogImages { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BlogPost>()
                .HasMany(bp => bp.Categories)
                .WithMany(c => c.BlogPosts)
                .UsingEntity(j => j.ToTable("BlogPostCategory"));

            base.OnModelCreating(modelBuilder);
        }


    }

}
