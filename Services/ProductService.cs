using Microsoft.EntityFrameworkCore;
using ProductCatalogApi.Data;
using ProductCatalogApi.DTOs;
using ProductCatalogApi.Models;

namespace ProductCatalogApi.Services;

// Service layer handles business logic
public class ProductService : IProductService
{
    private readonly AppDbContext _context;

    public ProductService(AppDbContext context)
    {
        _context = context;
    }

    // Get all products with their category information
    public async Task<IEnumerable<ProductDto>> GetAllProducts()
    {
        return await _context.Products
            .Include(p => p.Category) // Include related category data
            .Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                CategoryId = p.CategoryId,
                CategoryName = p.Category!.Name
            })
            .ToListAsync();
    }

    // Get a single product by ID
    public async Task<ProductDto?> GetProductById(int id)
    {
        return await _context.Products
            .Include(p => p.Category)
            .Where(p => p.Id == id)
            .Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                CategoryId = p.CategoryId,
                CategoryName = p.Category!.Name
            })
            .FirstOrDefaultAsync();
    }

    // Create a new product
    public async Task<ProductDto> CreateProduct(ProductDto productDto)
    {
        var product = new Product
        {
            Name = productDto.Name,
            Description = productDto.Description,
            Price = productDto.Price,
            CategoryId = productDto.CategoryId
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        productDto.Id = product.Id;
        return productDto;
    }

    // Update an existing product
    public async Task<bool> UpdateProduct(int id, ProductDto productDto)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return false;

        product.Name = productDto.Name;
        product.Description = productDto.Description;
        product.Price = productDto.Price;
        product.CategoryId = productDto.CategoryId;

        await _context.SaveChangesAsync();
        return true;
    }

    // Delete a product
    public async Task<bool> DeleteProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return false;

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return true;
    }
    public async Task<IEnumerable<ProductDto>> GetProductsFilteredByPrice(decimal minPrice)
    {
        return await _context.Products
            .Where(p => p.Price >= minPrice)
            .Include(p => p.Category)
            .Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                CategoryId = p.CategoryId,
                CategoryName = p.Category!.Name
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<ProductDto>> GetTopProductsByPrice(int n)
    {
        return await _context.Products
            .OrderByDescending(p => p.Price)
            .Take(n)
            .Include(p => p.Category)
            .Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                CategoryId = p.CategoryId,
                CategoryName = p.Category!.Name
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<ProductDto>> GetProductsByCategory(int categoryId)
    {
        return await _context.Products
            .Where(p => p.CategoryId == categoryId)
            .Include(p => p.Category)
            .Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                CategoryId = p.CategoryId,
                CategoryName = p.Category!.Name
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<ProductDto>> SearchProducts(string keyword)
    {
        keyword = keyword.ToLower();
        return await _context.Products
            .Where(p => p.Name.ToLower().Contains(keyword))
            .Include(p => p.Category)
            .Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                CategoryId = p.CategoryId,
                CategoryName = p.Category!.Name
            })
            .ToListAsync();
    }

    public async Task<decimal> GetAveragePrice()
    {
        if (!await _context.Products.AnyAsync())
            return 0;

        return await _context.Products.AverageAsync(p => p.Price);
    }

    public async Task<ProductDto?> GetCheapestProduct()
    {
        return await _context.Products
            .OrderBy(p => p.Price)
            .Include(p => p.Category)
            .Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                CategoryId = p.CategoryId,
                CategoryName = p.Category!.Name
            })
            .FirstOrDefaultAsync();
    }

    public async Task<ProductDto?> GetMostExpensiveProduct()
    {
        return await _context.Products
            .OrderByDescending(p => p.Price)
            .Include(p => p.Category)
            .Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                CategoryId = p.CategoryId,
                CategoryName = p.Category!.Name
            })
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<CategoryWithProductsDto>> GetProductsGroupedByCategory()
    {
        return await _context.Categories
            .Select(c => new CategoryWithProductsDto
            {
                CategoryId = c.Id,
                CategoryName = c.Name,
                Products = c.Products.Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    CategoryId = p.CategoryId,
                    CategoryName = c.Name
                }).ToList()
            })
            .ToListAsync();
    }
}
