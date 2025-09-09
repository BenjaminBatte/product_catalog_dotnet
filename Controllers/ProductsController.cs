using Microsoft.AspNetCore.Mvc;
using ProductCatalogApi.DTOs;
using ProductCatalogApi.Services;

namespace ProductCatalogApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly ProductService _productService;

    // Inject the ProductService via constructor
    public ProductsController(ProductService productService)
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
}