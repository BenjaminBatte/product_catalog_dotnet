using ProductCatalogApi.DTOs;

namespace ProductCatalogApi.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllProducts();
        Task<ProductDto?> GetProductById(int id);
        Task<ProductDto> CreateProduct(ProductDto productDto);
        Task<bool> UpdateProduct(int id, ProductDto productDto);
        Task<bool> DeleteProduct(int id);

        Task<IEnumerable<ProductDto>> GetProductsFilteredByPrice(decimal minPrice);
        Task<IEnumerable<ProductDto>> GetTopProductsByPrice(int n);
        Task<IEnumerable<ProductDto>> GetProductsByCategory(int categoryId);
        Task<IEnumerable<ProductDto>> SearchProducts(string keyword);
        Task<decimal> GetAveragePrice();
        Task<ProductDto?> GetCheapestProduct();
        Task<ProductDto?> GetMostExpensiveProduct();
        Task<IEnumerable<CategoryWithProductsDto>> GetProductsGroupedByCategory();

    }
}
