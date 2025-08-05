using Microsoft.EntityFrameworkCore;
using ProductCatalogApi.Data;
using ProductCatalogApi.DTOs;
using ProductCatalogApi.Models;

namespace ProductCatalogApi.Services;

// Service layer handles business logic
public class ProductService
{
    private readonly AppDbContext _context;

    // Inject the DbContext via constructor
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
}