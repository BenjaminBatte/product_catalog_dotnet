namespace ProductCatalogApi.DTOs
{
    public class CategoryWithProductsDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;

        // Embed products inside the category
        public List<ProductDto> Products { get; set; } = new();
    }
}
