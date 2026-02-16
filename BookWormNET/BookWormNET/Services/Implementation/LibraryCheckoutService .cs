using BookWormNET.Data;
using BookWormNET.dto;
using BookWormNET.Models;
using BookWormNET.Services.Interfaces;
using System;
using System.Linq;

namespace BookWormNET.Services.Implementation
{
    public class LibraryCheckoutService : ILibraryCheckoutService
    {
        private readonly BookwormDbContext _context;

        public LibraryCheckoutService(BookwormDbContext context)
        {
            _context = context;
        }

        public void Checkout(LibraryCheckoutRequest request)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {

                if (request.ProductIds == null || !request.ProductIds.Any())
                    throw new Exception("No books selected");


                // 1️⃣ User
                var user = _context.Users
                    .FirstOrDefault(u => u.UserId == request.UserId)
                    ?? throw new Exception("User not found");



                // 2️⃣ Library Package
                var package = _context.LibraryPackages
                    .FirstOrDefault(p => p.PackageId == request.PackageId)
                    ?? throw new Exception("Library package not found");



                if (!package.BookLimit.HasValue || package.BookLimit.Value <= 0)
                    throw new Exception("Invalid package book limit");



                var today = DateOnly.FromDateTime(DateTime.UtcNow);


                int activeBorrowedBooks = _context.MyLibraries
                    .Count(m => m.UserId == user.UserId && m.EndDate > today);



                int bookLimit = package.BookLimit ?? 0;


                if (activeBorrowedBooks + request.ProductIds.Count > bookLimit)
                    throw new Exception("Library book limit exceeded");



                var txn = new Transaction
                {
                    UserId = user.UserId,
                    TransactionType = TransactionType.BUY, // package is a purchase
                    Status = TransactionStatus.PENDING,
                    TotalAmount = package.Cost,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Transactions.Add(txn);
                _context.SaveChanges();
                //************************************


                //var avgBookPrice = package.Cost / (package.BookLimit ?? 1);

                var avgBookPrice = package.Cost / bookLimit;

                var purchase = new LibraryPackagePurchase
                {
                    TransactionId = txn.TransactionId,
                    UserId = user.UserId,
                    PackageId = package.PackageId,
                    PackagePrice = package.Cost,
                    AllowedBooks = package.BookLimit ?? 0,
                    AvgBookPrice = avgBookPrice,
                    PurchaseDate = DateTime.UtcNow
                };

                _context.LibraryPackagePurchases.Add(purchase);
                _context.SaveChanges();



                // 3️⃣ Active borrowed books (FIXED)
                

                //if (activeBorrowedBooks + request.ProductIds.Count > package.BookLimit)
                //    throw new Exception("Library book limit exceeded");


                //int bookLimit = package.BookLimit ?? 0;

                //if (activeBorrowedBooks + request.ProductIds.Count > bookLimit)
                //    throw new Exception("Library book limit exceeded");


                var startDate = today;
                var endDate = startDate.AddDays(package.ValidityDays ?? 0);

                //int selectedBooks = request.ProductIds.Count;

                int booksTakenSoFar = activeBorrowedBooks;

                

                // 6️⃣ Create MyLibrary per book
                foreach (var productId in request.ProductIds)
                {
                    var product = _context.Products
                        .FirstOrDefault(p => p.ProductId == productId)
                        ?? throw new Exception("Product not found");

                    //if (product.IsLibrary != 1)
                    //{
                    //    throw new Exception(
                    //        $"Product {product.ProductName} is not available for library");
                    //}

                    if (!product.IsLibrary.HasValue || product.IsLibrary.Value != 1UL)
                    {
                        throw new Exception(
                            $"Product {product.ProductName} is not available for library");
                    }

                    bool alreadyOwned = _context.MyLibraries.Any(m =>
                    m.UserId == user.UserId &&
                    m.ProductId == product.ProductId &&
                    m.EndDate > today

                    );

                    if (alreadyOwned)
                        throw new Exception($"{product.ProductName} already in library");

                    booksTakenSoFar++;
                    //*************************************
                    decimal royaltyPercent = product.RoyaltyPercent ?? 0m;
                    decimal royaltyAmount = avgBookPrice * royaltyPercent / 100m;

                    var purchaseItem = new LibraryPackagePurchaseItem
                    {
                        PurchaseId = purchase.PurchaseId,
                        ProductId = product.ProductId,
                        RoyaltyPercent = royaltyPercent,
                        RoyaltyAmount = royaltyAmount
                    };

                    _context.LibraryPackagePurchaseItems.Add(purchaseItem);
                    



                    if (royaltyPercent > 0)
                    {
                        var royalty = new RoyaltyCalculation
                        {
                            ItemId = purchaseItem.ItemId,           // IMPORTANT
                            ProductId = product.ProductId,
                            RoyaltyPercent = royaltyPercent,
                            TotalAmount = avgBookPrice,
                            TotalRoyalty = royaltyAmount,
                            RoycalTrandate = DateOnly.FromDateTime(DateTime.UtcNow)
                        };

                        _context.RoyaltyCalculations.Add(royalty);
                        _context.SaveChanges();

                        // 1️⃣ Get beneficiary
                        var beneficiary = _context.Beneficiaries
                            .FirstOrDefault(b => b.ProductId == product.ProductId);

                        if (beneficiary == null)
                            throw new Exception($"No beneficiary found for product {product.ProductId}");

                        // 2️⃣ Store royalty distribution
                        var productBeneficiary = new ProductBeneficiary
                        {
                            ProductId = product.ProductId,
                            BeneficiaryId = beneficiary.BeneficiaryId,
                            RoycalId = royalty.RoycalId,
                            RoyaltyReceived = royaltyAmount
                        };

                        _context.ProductBeneficiaries.Add(productBeneficiary);

                    }

                    //*********************************************8
                    _context.MyLibraries.Add(new MyLibrary
                    {
                        UserId = user.UserId,
                        PackageId = package.PackageId,
                        ProductId = product.ProductId,
                        StartDate = startDate,
                        EndDate = endDate,
                        BooksAllowed = package.BookLimit ?? 0,
                        BooksTaken = booksTakenSoFar
                    });
                }

                txn.Status = TransactionStatus.SUCCESS;

                _context.SaveChanges();
                transaction.Commit();
            }
            catch
            {   
                transaction.Rollback();
                throw;
            }
        }
    }
}
