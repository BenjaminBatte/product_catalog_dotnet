namespace ProductCatalogApi.DTOs;

// Category DTO used for API requests/responses
public class CategoryDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
}