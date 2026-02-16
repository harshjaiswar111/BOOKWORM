using BookWormNET.Data;
using BookWormNET.dto;
using BookWormNET.Models;
using Microsoft.EntityFrameworkCore;

namespace BookWormNET.Services.Implementation
{
    public class ShelfService : IShelfService
    {
        private readonly BookwormDbContext _context;

        public ShelfService(BookwormDbContext context)
        {
            _context = context;
        }



        //GET SHELF
        //public async Task<List<MyShelf>> GetUserShelfAsync(int userId)
        //{
        //    var now = DateTime.UtcNow;

        //    return await _context.MyShelves
        //        .Include(s => s.Product)
        //        .Where(s =>
        //            s.UserId == userId &&
        //            (s.ProductExpiryDate == null || s.ProductExpiryDate > now)
        //        )
        //        .OrderByDescending(s => s.ShelfId)
        //        .ToListAsync();
        //}


        

    public async Task<List<ShelfResponseDto>> GetUserShelfAsync(int userId)
    {
        var now = DateTime.UtcNow;

            //return await _context.MyShelves
            //    .Where(s =>
            //        s.UserId == userId &&
            //        (s.ProductExpiryDate == null || s.ProductExpiryDate > now)
            //    )
            //    .Select(s => new ShelfResponseDto
            //    {
            //        ShelfId = s.ShelfId,
            //        ProductExpiryDate = s.ProductExpiryDate,
            //        PurchaseType = s.ProductExpiryDate != null ? "rent" : "buy",

            //        Product = new ShelfProductDto
            //        {
            //            ProductId = s.Product.ProductId,
            //            ProductName = s.Product.ProductName,
            //            ProductImage = s.Product.ProductImage,

            //            Author = s.Product.ProductAuthorNavigation != null
            //                ? new ShelfAuthorDto
            //                {
            //                    Name = s.Product.ProductAuthorNavigation.Name
            //                }
            //                : null
            //        }
            //    })
            //    .OrderByDescending(s => s.ShelfId)
            //    .ToListAsync();

            return await _context.MyShelves
    .AsNoTracking()
    .Include(s => s.Product)
        .ThenInclude(p => p.ProductAuthorNavigation)
    .Where(s =>
        s.UserId == userId &&
        (s.ProductExpiryDate == null || s.ProductExpiryDate > now)
    )
    //.Select(s => new ShelfResponseDto
    //{
    //    ShelfId = s.ShelfId,
    //    ProductExpiryDate = s.ProductExpiryDate,
    //    PurchaseType = s.ProductExpiryDate != null ? "rent" : "buy",

    //    Product = new ShelfProductDto
    //    {
    //        ProductId = s.Product.ProductId,
    //        ProductName = s.Product.ProductName,
    //        ProductImage = s.Product.ProductImage,

    //        Author = s.Product.ProductAuthorNavigation != null
    //            ? new ShelfAuthorDto
    //            {
    //                Name = s.Product.ProductAuthorNavigation.Name
    //            }
    //            : null
    //    }
    //})
    .Select(s => new ShelfResponseDto
    {
        ShelfId = s.ShelfId,
        ProductExpiryDate = s.ProductExpiryDate,

        // ✅ source of truth
        PurchaseType = s.ProductExpiryDate != null ? "rent" : "buy",

        // ✅ alias for frontend (THIS FIXES EVERYTHING)
        Type = s.ProductExpiryDate != null ? "rent" : "buy",

        Product = new ShelfProductDto
        {
            ProductId = s.Product.ProductId,
            ProductName = s.Product.ProductName,
            ProductImage = s.Product.ProductImage,
            Author = s.Product.ProductAuthorNavigation != null
            ? new ShelfAuthorDto
            {
                Name = s.Product.ProductAuthorNavigation.Name
            }
            : null
        }
    })

    .OrderByDescending(s => s.ShelfId)
    .ToListAsync();

        }



        // ADD TO SHELF
        public async Task<MyShelf> AddToShelfAsync(int userId, int productId, int? rentDays)
        {
            
            bool exists = await _context.MyShelves
                .AnyAsync(s => s.UserId == userId && s.ProductId == productId);

            if (exists)
                throw new InvalidOperationException("Product already exists in shelf");

            var shelf = new MyShelf
            {
                UserId = userId,
                ProductId = productId,
                ProductExpiryDate = rentDays.HasValue && rentDays > 0
                    ? DateTime.UtcNow.AddDays(rentDays.Value)
                    : null
            };

            _context.MyShelves.Add(shelf);
            await _context.SaveChangesAsync();

            return shelf;
        }



        // DELETE FROM SHELF
        public async Task RemoveFromShelfAsync(int shelfId, int userId)
        {
            var shelfItem = await _context.MyShelves
                .FirstOrDefaultAsync(s => s.ShelfId == shelfId && s.UserId == userId);

            if (shelfItem == null)
                throw new KeyNotFoundException("Shelf item not found");

            _context.MyShelves.Remove(shelfItem);
            await _context.SaveChangesAsync();
        }


        public async Task<(byte[] PdfData, string FileName)> ReadBookAsync(int shelfId, int userId)
        {
            var now = DateTime.UtcNow;

            var shelfItem = await _context.MyShelves
                .Include(s => s.Product)
                    .ThenInclude(p => p.PdfBooks)
                .FirstOrDefaultAsync(s =>
                    s.ShelfId == shelfId &&
                    s.UserId == userId &&
                    (s.ProductExpiryDate == null || s.ProductExpiryDate > now)
                );

            if (shelfItem == null)
                throw new UnauthorizedAccessException("You cannot access this book");

            var pdf = shelfItem.Product.PdfBooks.FirstOrDefault();

            if (pdf == null || pdf.PdfData == null)
                throw new FileNotFoundException("PDF not found");

            return (pdf.PdfData, pdf.FileName ?? "book.pdf");
        }

    }
}
