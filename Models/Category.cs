namespace ProductCatalogApi.Models;

// Category model represents a product category
public class Category
{
    public int Id { get; set; } // Primary key
    
    // Category name (e.g., "Electronics", "Clothing")
    public required string Name { get; set; }
    
    // Description of the category
    public string? Description { get; set; }
    
    // Navigation property - collection of products in this category
    // This allows us to access all products that belong to this category
    public ICollection<Product> Products { get; set; } = new List<Product>();
}