using BookWormNET.Data;
using BookWormNET.Models;
using BookWormNET.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

[ApiController]
[Route("api/books")]
[Authorize]
public class BooksController : ControllerBase
{
    private readonly BookwormDbContext _context;
    private readonly IShelfService _shelfService;

    public BooksController(
        BookwormDbContext context,
        IShelfService shelfService)
    {
        _context = context;
        _shelfService = shelfService;
    }

    // 🔑 FRONTEND URL (UNCHANGED)
    // GET /api/books/{productId}/read
    [HttpGet("{productId:int}/read")]
    public async Task<IActionResult> ReadBook(int productId)
    {
        int userId = GetUserId();

        // 1️⃣ Find shelf entry for THIS user + product
        var shelfId = await _context.MyShelves
            .Where(s => s.UserId == userId && s.ProductId == productId)
            .OrderByDescending(s => s.ShelfId)
            .Select(s => s.ShelfId)
            .FirstOrDefaultAsync();

        if (shelfId == 0)
            return NotFound("Book not in your shelf");

        // 2️⃣ Reuse existing secure logic
        var (pdfData, _) =
            await _shelfService.ReadBookAsync(shelfId, userId);

        // 3️⃣ Stream inline (discourage download)
        Response.Headers.Add("Content-Disposition", "inline");
        Response.Headers.Add("Cache-Control", "no-store");

        return File(pdfData, "application/pdf");
    }

    // ✅ FIXED CLAIM LOGIC (matches your JWT)
    private int GetUserId()
    {
        var userIdClaim =
            User.FindFirst("UserId")?.Value ??
            User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim))
            throw new UnauthorizedAccessException("Invalid token");

        return int.Parse(userIdClaim);
    }


    [HttpPost("{productId:int}/upload")]
    //[Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> UploadPdf(
    int productId,
    IFormFile file,
    [FromServices] IPdfBookService pdfService)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded");

        if (file.ContentType != "application/pdf")
            return BadRequest("Only PDF files allowed");

        await pdfService.UploadPdfAsync(file, productId);

        return Ok("PDF uploaded successfully");
    }


}
