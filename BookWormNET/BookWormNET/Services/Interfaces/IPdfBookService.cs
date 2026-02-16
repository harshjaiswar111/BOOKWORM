using Microsoft.AspNetCore.Http;
using BookWormNET.Models;

public interface IPdfBookService
{
    Task UploadPdfAsync(IFormFile file, int productId);
    Task<PdfBook?> GetPdfByProductIdAsync(int productId);
}
