using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Repositories.Implementation
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _db;

        public CategoryRepository(ApplicationDbContext dbContext)
        {
            _db = dbContext;
        }

        public async Task<Category> CreateAsync(Category category)
        {
            await _db.Categories.AddAsync(category);
            await _db.SaveChangesAsync();

            return category;
        }

        public async Task<Category?> DeleteAsync(Guid id)
        {
            var exist = await _db.Categories.FirstOrDefaultAsync(x => x.Id == id);

            if(exist is null)
            {
                return null;
            }

            _db.Categories.Remove(exist);
            await _db.SaveChangesAsync();

            return exist;
        }

        public async Task<IEnumerable<Category>> GetAllAsync(
            string? query = null,
            string? sortBy = null,
            string? sortDirection = null,
            int? pageNumber = 1,
            int? pageSize = 100)
        {
            // Query
            var categories = _db.Categories.AsQueryable();

            // Filterting

            if (!string.IsNullOrWhiteSpace(query))
            {
                categories = categories.Where(x => x.Name.Contains(query));
            }

            // Sorting
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                if(string.Equals(sortBy, "Name", StringComparison.OrdinalIgnoreCase))
                {
                    var isAsc = string.Equals(sortDirection, "asc", StringComparison.OrdinalIgnoreCase)
                                        ? true : false;
                    categories = isAsc ? categories.OrderBy(x => x.Name)
                                : categories.OrderByDescending(x => x.Name);
                }

                else if (string.Equals(sortBy, "URL", StringComparison.OrdinalIgnoreCase))
                {
                    var isAsc = string.Equals(sortDirection, "asc", StringComparison.OrdinalIgnoreCase)
                                        ? true : false;
                    categories = isAsc ? categories.OrderBy(x => x.UrlHandle)
                                : categories.OrderByDescending(x => x.UrlHandle);
                }
            }

            // Pagination
            // pagenumber 1 pagesize 5 - skip 0, take 5
            // pagenumber 2 pagesize 5 - skip 5, take 5
            // pagenumber 3 pagesize 5 - skip 10, take 5

            var skipResult = (pageNumber - 1) * pageSize;
            categories = categories.Skip(skipResult ?? 0).Take(pageSize ?? 100);

            return await categories.ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(Guid id)
        {
            return await _db.Categories.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<int?> GetCount(string? query = null)
        {
            var categories = _db.Categories.AsQueryable();
            if (!string.IsNullOrWhiteSpace(query))
            {
                categories = categories.Where(x => x.Name.Contains(query));
            }
            return await categories.CountAsync();
        }

        public async Task<Category?> UpdateAsync(Category category)
        {
            var existingCategory = await _db.Categories.FirstOrDefaultAsync(x => x.Id == category.Id);

            if (existingCategory != null)
            {
                _db.Entry(existingCategory).CurrentValues.SetValues(category);
                await _db.SaveChangesAsync();
                return category;
            }
            return null;
        }
    }
}
