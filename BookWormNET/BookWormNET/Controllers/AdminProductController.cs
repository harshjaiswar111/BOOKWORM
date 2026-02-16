using BookWormNET.Data;
using BookWormNET.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/admin/products")]
[Authorize] // role optional if you're the only admin
public class AdminProductController : ControllerBase
{
    private readonly BookwormDbContext _context;

    public AdminProductController(BookwormDbContext context)
    {
        _context = context;
    }

    [HttpPost("{productId:int}/upload-pdf")]
    public async Task<IActionResult> UploadPdf(
        int productId,
        IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded");

        if (!file.ContentType.Contains("pdf"))
            return BadRequest("Only PDF files allowed");

        var product = await _context.Products
            .FirstOrDefaultAsync(p => p.ProductId == productId);

        if (product == null)
            return NotFound("Product not found");

        bool exists = await _context.PdfBooks
            .AnyAsync(p => p.ProductId == productId);

        if (exists)
            return BadRequest("PDF already exists for this product");

        using var ms = new MemoryStream();
        await file.CopyToAsync(ms);

        var pdf = new PdfBook
        {
            ProductId = productId,
            FileName = file.FileName,
            PdfData = ms.ToArray()
        };

        _context.PdfBooks.Add(pdf);
        await _context.SaveChangesAsync();

        return Ok("PDF uploaded successfully");
    }
}
