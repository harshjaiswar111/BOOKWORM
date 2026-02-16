using BookWormNET.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookWormNET.Controllers
{
    [ApiController]
    [Route("api/my-library")]
    public class MyLibraryController : ControllerBase
    {
        private readonly BookwormDbContext _context;

        public MyLibraryController(BookwormDbContext context)
        {
            _context = context;
        }

        // GET /api/my-library/user/2
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetMyLibrary(int userId)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            var libraryItems = await _context.MyLibraries
                .Include(m => m.Product)
                    .ThenInclude(p => p.ProductAuthorNavigation)
                .Where(m =>
                    m.UserId == userId &&
                    m.EndDate > today
                )
                .Select(m => new
                {
                    packageId = m.PackageId,
                    product = new
                    {
                        
                        productId = m.Product.ProductId,
                        productName = m.Product.ProductName,
                        productImage = m.Product.ProductImage,
                        author = new
                        {
                            name = m.Product.ProductAuthorNavigation != null
                ? m.Product.ProductAuthorNavigation.Name
                : null
                        }
                    },
                    startDate = m.StartDate,
                    endDate = m.EndDate
                })

                .ToListAsync();

            return Ok(libraryItems);
        }

        [HttpGet("read/{productId:int}")]
        [Authorize]
        public async Task<IActionResult> ReadLibraryBook(int productId)
        {
            var userIdClaim =
                User.FindFirst("UserId")?.Value ??
                User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized();

            int userId = int.Parse(userIdClaim);
            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            // 1️⃣ Verify user owns the book in library
            bool hasAccess = await _context.MyLibraries.AnyAsync(m =>
                m.UserId == userId &&
                m.ProductId == productId &&
                m.EndDate > today
            );

            if (!hasAccess)
                return NotFound("Book not available in your library");

            // 2️⃣ Fetch PDF from PdfBooks
            var pdf = await _context.PdfBooks
                .Where(pb => pb.ProductId == productId)
                .Select(pb => pb.PdfData)
                .FirstOrDefaultAsync();

            if (pdf == null)
                return NotFound("PDF not found");

            // 3️⃣ Stream inline
            Response.Headers.Add("Content-Disposition", "inline");
            Response.Headers.Add("Cache-Control", "no-store");

            return File(pdf, "application/pdf");
        }



    }
}
