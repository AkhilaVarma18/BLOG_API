using Blog.API.Data;
using Blog.API.Models.Domain;
using Blog.API.Reposittory.Interface;
using Microsoft.EntityFrameworkCore;

namespace Blog.API.Reposittory.Implementation
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly ApplicationDbContext dbContext;

        public BlogPostRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<BlogPost> CreateAsync(BlogPost blogpost)
        {
            // Check if blog post already exists
            var existingBlogPost = await dbContext.BlogPosts
                .Include(x => x.Categories)
                .FirstOrDefaultAsync(x => x.Id == blogpost.Id);

            if (existingBlogPost == null)
            {
                // New blog post - add it
                await dbContext.BlogPosts.AddAsync(blogpost);
            }
            else
            {
                // Update existing blog post
                dbContext.Entry(existingBlogPost).CurrentValues.SetValues(blogpost);

                // Clear existing categories and add new ones (avoid duplicates)
                existingBlogPost.Categories.Clear();

                // Get unique categories
                var uniqueCategories = blogpost.Categories
                    .GroupBy(c => c.Id)
                    .Select(g => g.First())
                    .ToList();

                foreach (var category in uniqueCategories)
                {
                    var existingCategory = await dbContext.Categories.FindAsync(category.Id);
                    if (existingCategory != null && !existingBlogPost.Categories.Any(c => c.Id == category.Id))
                    {
                        existingBlogPost.Categories.Add(existingCategory);
                    }
                }

                await dbContext.SaveChangesAsync();
                return existingBlogPost;
            }

            await dbContext.SaveChangesAsync();
            return blogpost;
        }
        //    await dbContext.BlogPosts.AddAsync(blogpost); 
        //    await dbContext.SaveChangesAsync();
        //    return blogpost;
        //}
        public async Task<BlogPost?> GetByUrlHandleAsync(string urlHandle)

        {


            return await dbContext.BlogPosts.Include(x => x.Categories).FirstOrDefaultAsync(x => x.UrlHandle == urlHandle);

        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            return await dbContext.BlogPosts.Include(x => x.Categories).ToListAsync();

        }
        public async Task<BlogPost?> GetByIdAsync(Guid id)
        {
            return await dbContext.BlogPosts.Include(x => x.Categories).FirstOrDefaultAsync(x => x.Id == id);
        }

    
    public async Task<BlogPost?> UpdateAsync(BlogPost blogPost)
        {
            var existingBlogPost = await dbContext.BlogPosts.Include(x => x.Categories).FirstOrDefaultAsync(x => x.Id == blogPost.Id);
            if(existingBlogPost == null)
            {
                return null;
            }
            dbContext.Entry(existingBlogPost).CurrentValues.SetValues(blogPost);

            existingBlogPost.Categories = blogPost.Categories;

            await dbContext.SaveChangesAsync(); return blogPost;

        }
        public async Task<BlogPost?> DeleteAsync(Guid id)
        {
            var existingBlogPost = await dbContext.BlogPosts.Include(x => x.Categories).FirstOrDefaultAsync(x => x.Id == id);
            if (existingBlogPost != null)
            {
                dbContext.BlogPosts.Remove(existingBlogPost);
                await dbContext.SaveChangesAsync();
                return existingBlogPost;
            }
            return null;
        }



    }        
    
}


