namespace ProductCatalogApi.Models;

// Product model represents an item in our catalog
public class Product
{
    public int Id { get; set; } // Primary key
    
    // Product name (e.g., "Smartphone X")
    public required string Name { get; set; }
    
    // Product description
    public string? Description { get; set; }
    
    // Price of the product
    public decimal Price { get; set; }
    
    // Foreign key to Category
    public int CategoryId { get; set; }
    
    // Navigation property - reference to the Category this product belongs to
    public Category? Category { get; set; }
}