using ProductCatalogApi.DTOs;

namespace ProductCatalogApi.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllCategories();
        Task<CategoryDto?> GetCategoryById(int id);
        Task<CategoryDto> CreateCategory(CategoryDto categoryDto);
        Task<bool> UpdateCategory(int id, CategoryDto categoryDto);
        Task<bool> DeleteCategory(int id);
        Task<IEnumerable<CategoryWithCountDto>> GetCategoriesWithProductCounts();
        Task<IEnumerable<CategoryWithAveragePriceDto>> GetCategoriesWithAveragePrices();
        Task<IEnumerable<CategoryWithFlagDto>> GetCategoriesWithExpensiveProducts(decimal minPrice);
        Task<IEnumerable<CategoryWithCountDto>> GetTopCategories(int n);
    }
}
