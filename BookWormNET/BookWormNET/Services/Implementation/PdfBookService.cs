using BookWormNET.Data;
using BookWormNET.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

public class PdfBookService : IPdfBookService
{
    private readonly BookwormDbContext _context;

    public PdfBookService(BookwormDbContext context)
    {
        _context = context;
    }

    // Java: savePdf(MultipartFile file, int productId)
    public async Task UploadPdfAsync(IFormFile file, int productId)
    {
        // 1️⃣ Validate product
        var product = await _context.Products
            .FirstOrDefaultAsync(p => p.ProductId == productId)
            ?? throw new Exception("Book not found");

        // 2️⃣ Read PDF bytes
        byte[] pdfData;
        using (var ms = new MemoryStream())
        {
            await file.CopyToAsync(ms);
            pdfData = ms.ToArray();
        }

        // 3️⃣ Insert or update PdfBook
        var existing = await _context.PdfBooks
            .FirstOrDefaultAsync(p => p.ProductId == productId);

        if (existing != null)
        {
            existing.PdfData = pdfData;
            existing.FileName = file.FileName;
        }
        else
        {
            _context.PdfBooks.Add(new PdfBook
            {
                ProductId = productId,
                PdfData = pdfData,
                FileName = file.FileName,
                Product = product
            });
        }

        await _context.SaveChangesAsync();
    }

    // Java: getPdfByProductId(int productId)
    public async Task<PdfBook?> GetPdfByProductIdAsync(int productId)
    {
        return await _context.PdfBooks
            .FirstOrDefaultAsync(p => p.ProductId == productId);
    }
}
