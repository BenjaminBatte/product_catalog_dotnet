namespace ProductCatalogApi.DTOs
{
    public class CategoryWithFlagDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool HasExpensiveProducts { get; set; }
    }
}
