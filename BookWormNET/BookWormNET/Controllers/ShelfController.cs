using BookWormNET.dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController]
[Route("api/shelf")]
[Authorize]
public class ShelfController : ControllerBase
{
    private readonly IShelfService _shelfService;

    public ShelfController(IShelfService shelfService)
    {
        _shelfService = shelfService;
    }

    
    [HttpGet]
    public async Task<IActionResult> GetMyShelf()
    {
        int userId = GetUserId();
        var shelfItems = await _shelfService.GetUserShelfAsync(userId);
        return Ok(shelfItems);
    }

    // ✅ POST: api/shelf
    [HttpPost]
    public async Task<IActionResult> AddToShelf([FromBody] AddToShelfRequest request)
    {
        int userId = GetUserId();

        var shelfItem = await _shelfService.AddToShelfAsync(
            userId,
            request.ProductId,
            request.RentDays
        );

        return Ok(shelfItem);
    }


    // ✅ GET: api/shelf/{userId}
    [HttpGet("{userId:int}")]
    public async Task<IActionResult> GetShelfByUserId(int userId)
    {
        var shelfItems = await _shelfService.GetUserShelfAsync(userId);
        return Ok(shelfItems);
    }


    [HttpGet("read/{shelfId:int}")]
    public async Task<IActionResult> ReadBook(int shelfId)
    {
        int userId = GetUserId();

        var (pdfData, _) =
            await _shelfService.ReadBookAsync(shelfId, userId);

        Response.Headers.Add("Content-Disposition", "inline");
        Response.Headers.Add("X-Content-Type-Options", "nosniff");
        Response.Headers.Add("Cache-Control", "no-store, no-cache, must-revalidate");
        Response.Headers.Add("Pragma", "no-cache");
        Response.Headers.Add("Expires", "0");

        return File(pdfData, "application/pdf");
    }



    // ✅ DELETE: api/shelf/{shelfId}
    //[HttpDelete("{shelfId:int}")]
    //public async Task<IActionResult> RemoveFromShelf(int shelfId)
    //{
    //    int userId = GetUserId();
    //    await _shelfService.RemoveFromShelfAsync(shelfId, userId);
    //    return NoContent();
    //}

    // 🔒 JWT helper
    private int GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim))
            throw new UnauthorizedAccessException("Invalid user token");

        return int.Parse(userIdClaim);
    }
}