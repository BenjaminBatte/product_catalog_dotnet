using Microsoft.EntityFrameworkCore;
using ProductCatalogApi.Data;
using ProductCatalogApi.DTOs;
using ProductCatalogApi.Models;

namespace ProductCatalogApi.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;

        public CategoryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategories()
        {
            return await _context.Categories
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description
                })
                .ToListAsync();
        }

        public async Task<CategoryDto?> GetCategoryById(int id)
        {
            return await _context.Categories
                .Where(c => c.Id == id)
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description
                })
                .FirstOrDefaultAsync();
        }

        public async Task<CategoryDto> CreateCategory(CategoryDto categoryDto)
        {
            var category = new Category
            {
                Name = categoryDto.Name,
                Description = categoryDto.Description
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            categoryDto.Id = category.Id;
            return categoryDto;
        }

        public async Task<bool> UpdateCategory(int id, CategoryDto categoryDto)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return false;

            category.Name = categoryDto.Name;
            category.Description = categoryDto.Description;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteCategory(int id)
        {
            var category = await _context.Categories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null) return false;
            if (category.Products.Any()) return false; // Can't delete if category has products

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<CategoryWithCountDto>> GetCategoriesWithProductCounts()
        {
            return await _context.Categories
                .Select(c => new CategoryWithCountDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    ProductCount = c.Products.Count
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<CategoryWithAveragePriceDto>> GetCategoriesWithAveragePrices()
        {
            return await _context.Categories
                .Select(c => new CategoryWithAveragePriceDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    AveragePrice = c.Products.Any() ? c.Products.Average(p => p.Price) : 0
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<CategoryWithFlagDto>> GetCategoriesWithExpensiveProducts(decimal minPrice)
        {
            return await _context.Categories
                .Select(c => new CategoryWithFlagDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    HasExpensiveProducts = c.Products.Any(p => p.Price >= minPrice)
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<CategoryWithCountDto>> GetTopCategories(int n)
        {
            return await _context.Categories
                .Select(c => new CategoryWithCountDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    ProductCount = c.Products.Count
                })
                .OrderByDescending(c => c.ProductCount)
                .Take(n)
                .ToListAsync();
        }
    }
}