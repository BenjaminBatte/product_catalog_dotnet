namespace ProductCatalogApi.DTOs
{
    public class CategoryWithCountDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int ProductCount { get; set; }
    }
}
