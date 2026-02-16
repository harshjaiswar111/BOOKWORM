using BookWormNET.dto;
using BookWormNET.Models;

public interface IShelfService
{
    Task<List<ShelfResponseDto>> GetUserShelfAsync(int userId);
    Task<MyShelf> AddToShelfAsync(int userId, int productId, int? rentDays);
    Task RemoveFromShelfAsync(int shelfId, int userId);
    Task<(byte[] PdfData, string FileName)> ReadBookAsync(int shelfId, int userId);

}