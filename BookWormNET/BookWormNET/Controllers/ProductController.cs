using BookWormNET.Models;
using BookWormNET.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _service;

    public ProductsController(IProductService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _service.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await _service.GetByIdAsync(id);
        if (product == null)
            return NotFound();

        return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Product product)
    {
        var created = await _service.AddAsync(product);
        return CreatedAtAction(nameof(GetById), new { id = created.ProductId }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Product product)
    {
        var updated = await _service.UpdateAsync(id, product);
        if (updated == null)
            return NotFound();

        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);
        if (!deleted)
            return NotFound();

        return NoContent();
    }


    [HttpGet("library")]
    public async Task<IActionResult> GetLibraryProducts()
    {
        var libraryBooks = await _service.GetLibraryProductsAsync();
        return Ok(libraryBooks);
    }

    [HttpGet("by-genere/{genereDesc}")]
    public IActionResult GetByGenere(string genereDesc)
    {
        var products = _service.GetByGenereDesc(genereDesc);

        if (products == null || products.Count == 0)
            return NotFound("No products found for this genere");

        return Ok(products);
    }


    [HttpGet("by-language/{languageDesc}")]
    public IActionResult GetByLanguage(string languageDesc)
    {
        var products = _service.GetByLanguageDesc(languageDesc);

        if (products == null || products.Count == 0)
            return NotFound("No products found for this language");

        return Ok(products);
    }


    [HttpGet("search")]
    public async Task<IActionResult> SearchByProductName([FromQuery] string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return BadRequest("Search term is required");

        var products = await _service.SearchByProductNameAsync(name);

        if (!products.Any())
            return NotFound("No products found");

        return Ok(products);
    }


    [HttpGet("filter")]
    public async Task<IActionResult> Filter([FromQuery] string? genere, [FromQuery] string? language)
    {
        var result = await _service.FilterAsync(genere, language);
        return Ok(result);
    }


}