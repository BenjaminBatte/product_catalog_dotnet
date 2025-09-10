namespace ProductCatalogApi.DTOs
{
    public class CategoryWithAveragePriceDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal AveragePrice { get; set; }
    }
}
