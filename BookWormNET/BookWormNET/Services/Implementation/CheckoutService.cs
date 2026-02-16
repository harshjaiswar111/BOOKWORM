using BookWormNET.Data;
using BookWormNET.dto;
using BookWormNET.Models;
using BookWormNET.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;


namespace BookWormNET.Services.Implementation
{
    public class CheckoutService : ICheckoutService
    {
        private readonly BookwormDbContext _context;
        private readonly TransactionPdfService _pdfService;
        private readonly EmailService _emailService;
        public CheckoutService(BookwormDbContext context,
            TransactionPdfService pdfService,
            EmailService emailService)
        {
            _context = context;
            _pdfService = pdfService;
            _emailService = emailService;
        }

        public async Task CheckoutAsync(int userId, CheckoutRequestDto request)
        {
            Transaction transaction = null!;
            List<TransactionItem> items = new();

            if (request.ProductIds == null || !request.ProductIds.Any())
                throw new Exception("No products selected");

            using var dbTransaction =
                await _context.Database.BeginTransactionAsync();

            try
            {
                // 1️⃣ Validate user
                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);


                if (user == null)
                    throw new Exception("User not found");

                // 2️⃣ Fetch products
                var products = await _context.Products
                    .Where(p => request.ProductIds.Contains(p.ProductId))
                    .ToListAsync();

                if (products.Count != request.ProductIds.Count)
                    throw new Exception("One or more products are invalid");

                //var isRent = request.RentDays.HasValue && request.RentDays > 0;

                //var transactionType = isRent
                //    ? TransactionType.RENT
                //    : TransactionType.BUY;

                //var transactionType = request.TransactionType;

                //            var transactionType =
                //request.TransactionType == TransactionType.RENT
                //    ? TransactionType.RENT
                //    : TransactionType.BUY;


                var transactionType =
                request.RentDays.HasValue && request.RentDays > 0
                 ? TransactionType.RENT
                 : TransactionType.BUY;


                if (transactionType == TransactionType.RENT &&
                    (!request.RentDays.HasValue || request.RentDays <= 0))
                {
                    throw new Exception("RentDays is required for RENT transaction");
                }



                //if (request.TransactionType == TransactionType.RENT &&
                //    (!request.RentalDays.HasValue || request.RentalDays <= 0))
                //{
                //    throw new Exception("Rental days are required for renting products");
                //}


                // 3️⃣ Create transaction (PENDING)
                transaction = new Transaction
                {
                    UserId = user.UserId,
                    Status = TransactionStatus.PENDING,
                    TransactionType = transactionType,
                    CreatedAt = DateTime.UtcNow
                };

                await _context.Transactions.AddAsync(transaction);
                await _context.SaveChangesAsync();

                // 4️⃣ Calculate total
                transaction.TotalAmount =
                    products.Sum(p =>
                        GetEffectivePrice(
                            p,
                            transactionType,
                            request.RentDays
                        )
                    );



                await _context.SaveChangesAsync();

                
                foreach (var product in products)
                {
                    //await _context.TransactionItems.AddAsync(new TransactionItem
                    //{
                    //    TransactionId = transaction.TransactionId,
                    //    ProductId = product.ProductId,
                    //    Price = GetEffectivePrice(
                    //                product,
                    //                transactionType,
                    //                request.RentDays
                    //    ),
                    //    Quantity = 1
                    //});



                    var itemPrice = GetEffectivePrice(
                        product,
                        transactionType,
                        request.RentDays
                    );

                    // 2️⃣ Create transaction item
                    var transactionItem = new TransactionItem
                    {
                        TransactionId = transaction.TransactionId,
                        ProductId = product.ProductId,
                        Price = itemPrice,
                        Quantity = 1
                    };

                    await _context.TransactionItems.AddAsync(transactionItem);
                    await _context.SaveChangesAsync();
                    items.Add(transactionItem);

                    if (product.RoyaltyPercent.HasValue && product.RoyaltyPercent.Value > 0)
                    {
                        decimal totalRoyalty =
                            itemPrice * product.RoyaltyPercent.Value / 100m;

                        var royaltyCalculation = new RoyaltyCalculation
                        {
                            ItemId = transactionItem.ItemId,            // invdtl_id
                            ProductId = product.ProductId,
                            RoycalTrandate = DateOnly.FromDateTime(DateTime.UtcNow),
                            TotalAmount = itemPrice,
                            RoyaltyPercent = product.RoyaltyPercent,
                            TotalRoyalty = totalRoyalty
                        };

                        await _context.RoyaltyCalculations.AddAsync(royaltyCalculation);
                        await _context.SaveChangesAsync();

                        var beneficiary = await _context.Beneficiaries
                            .FirstOrDefaultAsync(b => b.ProductId == product.ProductId);

                        if (beneficiary == null)
                            throw new Exception($"No beneficiary found for product {product.ProductId}");

                        // =====================================================
                        // ✅ STEP 3B: Store royalty in ProductBeneficiary table
                        // =====================================================
                        var productBeneficiary = new ProductBeneficiary
                        {
                            ProductId = product.ProductId,
                            BeneficiaryId = beneficiary.BeneficiaryId,
                            RoycalId = royaltyCalculation.RoycalId,
                            RoyaltyReceived = totalRoyalty
                        };

                        await _context.ProductBeneficiaries.AddAsync(productBeneficiary);
                        await _context.SaveChangesAsync();
                    }





                    bool alreadyOwned = await _context.MyShelves.AnyAsync(ms =>
                        ms.UserId == user.UserId &&
                        ms.ProductId == product.ProductId &&
                        ms.ProductExpiryDate == null   // permanent ownership
                    );

                    if (!alreadyOwned)
                    {
                        var shelfItem = new MyShelf
                        {
                            UserId = user.UserId,
                            ProductId = product.ProductId
                        };

                        if (transactionType == TransactionType.RENT)
                        {
                            shelfItem.ProductExpiryDate =
                                DateTime.UtcNow.AddDays(request.RentDays!.Value);
                        }
                        else
                        {
                            // BUY → permanent ownership
                            shelfItem.ProductExpiryDate = null;
                        }

                        await _context.MyShelves.AddAsync(shelfItem);
                    }
                }

                await _context.SaveChangesAsync();


                var cartItemsToRemove = await _context.Carts
                    .Where(c =>
                        c.UserId == user.UserId &&
                        request.ProductIds.Contains(c.ProductId)
                    )
                    .ToListAsync();

                _context.Carts.RemoveRange(cartItemsToRemove);

                await _context.SaveChangesAsync();

                transaction.Status = TransactionStatus.SUCCESS;
                await _context.SaveChangesAsync();
                await dbTransaction.CommitAsync();
            }
            catch
            {
                await dbTransaction.RollbackAsync();
                throw;
            }

            // 7️⃣ Generate PDF
            byte[] pdf = _pdfService.GenerateInvoice(transaction, items);

            // 8️⃣ Send Email
            _emailService.SendTransactionSuccessEmail(
                transaction.User.UserEmail,
                transaction,
                pdf
            );
        }

        private decimal GetEffectivePrice(
        Product product,
        TransactionType transactionType,
        int? rentalDays)
        {
            if (transactionType == TransactionType.RENT)
            {
                // Validate rentable
                if (product.IsRentable != 1)
                    throw new Exception($"Product '{product.ProductName}' is not rentable");

                if (!product.RentPerDay.HasValue)
                    throw new Exception($"Rent price missing for '{product.ProductName}'");

                if (!rentalDays.HasValue || rentalDays.Value <= 0)
                    throw new Exception("Invalid rental duration");

                if (product.MinRentDays.HasValue &&
                    rentalDays.Value < product.MinRentDays.Value)
                {
                    throw new Exception(
                        $"Minimum rental period for '{product.ProductName}' is {product.MinRentDays} days"
                    );
                }

                return product.RentPerDay.Value * rentalDays.Value;
            }

            // BUY logic
            if (product.ProductOfferprice.HasValue &&
                product.ProductOfferprice.Value > 0 &&  
                (!product.ProductOffPriceExpirydate.HasValue ||
                 product.ProductOffPriceExpirydate.Value >= DateOnly.FromDateTime(DateTime.UtcNow)))
            {
                return product.ProductOfferprice.Value;
            }

            return product.ProductBaseprice;
        }
    }
}
