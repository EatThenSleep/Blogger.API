using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Repositories.Implementation
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly ApplicationDbContext _db;
        public BlogPostRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<BlogPost> CreateAsync(BlogPost blogPost)
        {
            await _db.AddAsync(blogPost);
            await _db.SaveChangesAsync();
            return blogPost;
        }

        public async Task<BlogPost?> DeleteAsync(Guid id)
        {
            var existingBlogPost = await _db.BlogPosts.FirstOrDefaultAsync(x => x.Id == id);

            if (existingBlogPost != null)
            {
                _db.BlogPosts.Remove(existingBlogPost);
                await _db.SaveChangesAsync();
                return existingBlogPost;
            }
            return null;
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            return await _db.BlogPosts
                .Include("Categories")
                .ToListAsync();
        }

        public async Task<BlogPost?> GetByIdAsync(Guid id)
        {
            return await _db.BlogPosts.Include("Categories").FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<BlogPost?> GetByUrlAsync(string urlHandle)
        {
            return await _db.BlogPosts.Include("Categories")
                .FirstOrDefaultAsync(p => p.UrlHandle == urlHandle);
        }

        public async Task<BlogPost?> UpdateAsync(BlogPost blogPost)
        {
            var existingBlogPost = await _db.BlogPosts
                                    .Include("Categories")
                                    .FirstOrDefaultAsync(x => x.Id == blogPost.Id);
            if (existingBlogPost is null)
            {
                return null;
            }

            // Update Blog Post
            _db.Entry(existingBlogPost).CurrentValues.SetValues(blogPost);

            // Update Category
            existingBlogPost.Categories = blogPost.Categories;

            await _db.SaveChangesAsync();

            return blogPost;
        }


    }
}
