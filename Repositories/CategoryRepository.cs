using BookStore.Repositories.Interfaces;
using BookStore.Models;
using Microsoft.EntityFrameworkCore;
using BookStore.Context;

namespace BookStore.Repositories{
    public class CategoryRepository : ICategoryRepository
    {
       private readonly BookDbContext _context;

        public CategoryRepository(BookDbContext context)
        {
            _context = context;
        }

        public async Task<Category> GetCategoryByNameAsync(string name, CancellationToken cancellationToken)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.Name == name, cancellationToken);
        }

        public async Task<Category> AddCategoryAsync(Category category, CancellationToken cancellationToken)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync(cancellationToken);
            return category;
        }

        public async Task<IEnumerable<Category>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Categories.ToListAsync(cancellationToken);
        }
    }
}