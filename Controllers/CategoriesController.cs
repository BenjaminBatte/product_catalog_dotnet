using Microsoft.AspNetCore.Mvc;
using ProductCatalogApi.DTOs;
using ProductCatalogApi.Services;

namespace ProductCatalogApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }
    // GET: api/Categories
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories()
    {
        return Ok(await _categoryService.GetAllCategories());
    }

    // GET: api/Categories/5
    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryDto>> GetCategory(int id)
    {
        var category = await _categoryService.GetCategoryById(id);

        if (category == null)
        {
            return NotFound();
        }

        return category;
    }

    // PUT: api/Categories/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutCategory(int id, CategoryDto categoryDto)
    {
        if (id != categoryDto.Id)
        {
            return BadRequest();
        }

        var success = await _categoryService.UpdateCategory(id, categoryDto);
        if (!success)
        {
            return NotFound();
        }

        return NoContent();
    }

    // POST: api/Categories
    [HttpPost]
    public async Task<ActionResult<CategoryDto>> PostCategory(CategoryDto categoryDto)
    {
        var createdCategory = await _categoryService.CreateCategory(categoryDto);
        return CreatedAtAction(nameof(GetCategory), new { id = createdCategory.Id }, createdCategory);
    }

    // DELETE: api/Categories/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var success = await _categoryService.DeleteCategory(id);
        if (!success)
        {
            return BadRequest("Category not found or contains products");
        }

        return NoContent();
    }

    // GET: api/Categories/with-counts
    [HttpGet("with-counts")]
    public async Task<ActionResult<IEnumerable<CategoryWithCountDto>>> GetCategoriesWithCounts()
    {
        return Ok(await _categoryService.GetCategoriesWithProductCounts());
    }

    // GET: api/Categories/average-prices
    [HttpGet("average-prices")]
    public async Task<ActionResult<IEnumerable<CategoryWithAveragePriceDto>>> GetCategoriesWithAveragePrices()
    {
        return Ok(await _categoryService.GetCategoriesWithAveragePrices());
    }

    // GET: api/Categories/with-expensive-products?minPrice=100
    [HttpGet("with-expensive-products")]
    public async Task<ActionResult<IEnumerable<CategoryWithFlagDto>>> GetCategoriesWithExpensiveProducts([FromQuery] decimal minPrice)
    {
        return Ok(await _categoryService.GetCategoriesWithExpensiveProducts(minPrice));
    }

    // GET: api/Categories/top?n=3
    [HttpGet("top")]
    public async Task<ActionResult<IEnumerable<CategoryWithCountDto>>> GetTopCategories([FromQuery] int n = 5)
    {
        return Ok(await _categoryService.GetTopCategories(n));
    }

}