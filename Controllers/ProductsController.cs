using Microsoft.AspNetCore.Mvc;
using ProductCatalogApi.DTOs;
using ProductCatalogApi.Services;

namespace ProductCatalogApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    // Inject the ProductService via constructor
    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    // GET: api/Products
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
    {
        return Ok(await _productService.GetAllProducts());
    }

    // GET: api/Products/
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductDto>> GetProduct(int id)
    {
        var product = await _productService.GetProductById(id);

        if (product == null)
        {
            return NotFound();
        }

        return product;
    }

    // PUT: api/Products/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutProduct(int id, ProductDto productDto)
    {
        if (id != productDto.Id)
        {
            return BadRequest();
        }

        var success = await _productService.UpdateProduct(id, productDto);
        if (!success)
        {
            return NotFound();
        }

        return NoContent();
    }

    // POST: api/Products
    [HttpPost]
    public async Task<ActionResult<ProductDto>> PostProduct(ProductDto productDto)
    {
        var createdProduct = await _productService.CreateProduct(productDto);
        return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.Id }, createdProduct);
    }

    // DELETE: api/Products/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var success = await _productService.DeleteProduct(id);
        if (!success)
        {
            return NotFound();
        }

        return NoContent();
    }
    // GET: api/Products/filter?minPrice=50
    [HttpGet("filter")]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetFilteredProducts([FromQuery] decimal minPrice)
    {
        var products = await _productService.GetProductsFilteredByPrice(minPrice);
        return Ok(products);
    }
    [HttpGet("top")]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetTopProducts([FromQuery] int n = 5)
    {
        return Ok(await _productService.GetTopProductsByPrice(n));
    }

    // GET: api/Products/by-category/2
    [HttpGet("by-category/{categoryId}")]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductsByCategory(int categoryId)
    {
        return Ok(await _productService.GetProductsByCategory(categoryId));
    }

    // GET: api/Products/search?keyword=phone
    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<ProductDto>>> SearchProducts([FromQuery] string keyword)
    {
        return Ok(await _productService.SearchProducts(keyword));
    }

    // GET: api/Products/average-price
    [HttpGet("average-price")]
    public async Task<ActionResult<decimal>> GetAveragePrice()
    {
        return Ok(await _productService.GetAveragePrice());
    }

    // GET: api/Products/cheapest
    [HttpGet("cheapest")]
    public async Task<ActionResult<ProductDto?>> GetCheapestProduct()
    {
        return Ok(await _productService.GetCheapestProduct());
    }

    // GET: api/Products/most-expensive
    [HttpGet("most-expensive")]
    public async Task<ActionResult<ProductDto?>> GetMostExpensiveProduct()
    {
        return Ok(await _productService.GetMostExpensiveProduct());
    }

    // GET: api/Products/grouped-by-category
    [HttpGet("grouped-by-category")]
    public async Task<ActionResult<IEnumerable<CategoryWithProductsDto>>> GetProductsGroupedByCategory()
    {
        return Ok(await _productService.GetProductsGroupedByCategory());
    }

}