using BookWormNET.Data;
using BookWormNET.dto;
using BookWormNET.Models;
using BookWormNET.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookWormNET.Services.Implementation
{
    public class ProductService : IProductService
    {
        private readonly BookwormDbContext _context;

        public ProductService(BookwormDbContext context)
        {
            _context = context;
        }


        public async Task<Product> AddAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }







        //public async Task<IEnumerable<ProductResponseDto>> GetAllAsync()
        //{
        //    return await _context.Products
        //        .Select(p => new ProductResponseDto
        //        {
        //            ProductId = p.ProductId,
        //            ProductName = p.ProductName,
        //            ProductImage = p.ProductImage,

        //            ProductBaseprice = p.ProductBaseprice,
        //            ProductOfferprice = p.ProductOfferprice,
        //            DiscountPercent = p.DiscountPercent,

        //            Rentable = p.IsRentable == 1,
        //            RentPerDay = p.RentPerDay,
        //            MinRentDays = p.MinRentDays,

        //            Author = p.ProductAuthorNavigation != null
        //                ? new AuthorDto
        //                {
        //                    Name = p.ProductAuthorNavigation.Name
        //                }
        //                : null,

        //            ProductType = p.ProductTypeNavigation != null
        //                ? new ProductTypeDto
        //                {
        //                    TypeDesc = p.ProductTypeNavigation.TypeDesc
        //                }
        //                : null
        //        })
        //        .ToListAsync();
        //}







        public async Task<IEnumerable<ProductResponseDto>> GetAllAsync()
        {
            return await _context.Products
                .Select(p => new ProductResponseDto
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    ProductImage = p.ProductImage,

                    ProductBaseprice = p.ProductBaseprice,
                    ProductOfferprice = p.ProductOfferprice,
                    DiscountPercent = p.DiscountPercent,

                    Rentable = p.IsRentable == 1,
                    RentPerDay = p.RentPerDay,
                    MinRentDays = p.MinRentDays,
                    IsLibrary = p.IsLibrary,

                    ProductIsbn = p.ProductIsbn,
                    ProductDescriptionLong = p.ProductDescriptionLong,

                    Author = p.ProductAuthorNavigation != null
                        ? new Author
                        {
                            Name = p.ProductAuthorNavigation.Name
                        }
                        : null,

                    ProductType = p.ProductTypeNavigation != null
                        ? new ProductTypeMaster
                        {
                            TypeDesc = p.ProductTypeNavigation.TypeDesc
                        }
                        : null,

                    Language = p.ProductLangNavigation != null
                        ? new Language
                        {
                            LanguageDesc = p.ProductLangNavigation.LanguageDesc
                        }
                        : null,

                    Genere = p.ProductGenereNavigation != null
                        ? new Genere
                        {
                            GenereDesc = p.ProductGenereNavigation.GenereDesc
                        }
                        : null,

                    Publisher = p.ProductPublisherNavigation != null
                        ? new Publisher
                        {
                            Name = p.ProductPublisherNavigation.Name,
                            Email = p.ProductPublisherNavigation.Email
                        }
                        : null
                })
                .ToListAsync();
        }





        //public async Task<ProductResponseDto?> GetByIdAsync(int id)
        //{
        //    return await _context.Products
        //        .Where(p => p.ProductId == id)
        //        .Select(p => new ProductResponseDto
        //        {
        //            ProductId = p.ProductId,
        //            ProductName = p.ProductName,
        //            ProductImage = p.ProductImage,

        //            ProductBaseprice = p.ProductBaseprice,
        //            ProductOfferprice = p.ProductOfferprice,
        //            DiscountPercent = p.DiscountPercent,

        //            Rentable = p.IsRentable == 1,
        //            RentPerDay = p.RentPerDay,
        //            MinRentDays = p.MinRentDays,

        //            Author = p.ProductAuthorNavigation != null
        //                ? new Author
        //                {
        //                    Name = p.ProductAuthorNavigation.Name
        //                }
        //                : null,

        //            ProductType = p.ProductTypeNavigation != null
        //                ? new ProductTypeMaster
        //                {
        //                    TypeDesc = p.ProductTypeNavigation.TypeDesc
        //                }
        //                : null
        //        })
        //        .FirstOrDefaultAsync();
        //}



        public async Task<ProductResponseDto?> GetByIdAsync(int id)
        {
            return await _context.Products
                .Where(p => p.ProductId == id)
                .Select(p => new ProductResponseDto
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    ProductImage = p.ProductImage,

                    ProductBaseprice = p.ProductBaseprice,
                    ProductOfferprice = p.ProductOfferprice,
                    DiscountPercent = p.DiscountPercent,

                    Rentable = p.IsRentable == 1,
                    IsLibrary = p.IsLibrary,
                    RentPerDay = p.RentPerDay,
                    MinRentDays = p.MinRentDays,

                    ProductIsbn = p.ProductIsbn,
                    ProductDescriptionLong = p.ProductDescriptionLong,

                    Author = p.ProductAuthorNavigation == null ? null : new Author
                    {
                        AuthorId = p.ProductAuthorNavigation.AuthorId,
                        Name = p.ProductAuthorNavigation.Name
                    },

                    Language = p.ProductLangNavigation == null ? null : new Language
                    {
                        LanguageId = p.ProductLangNavigation.LanguageId,
                        LanguageDesc = p.ProductLangNavigation.LanguageDesc
                    },

                    Genere = p.ProductGenereNavigation == null ? null : new Genere
                    {
                        GenereId = p.ProductGenereNavigation.GenereId,
                        GenereDesc = p.ProductGenereNavigation.GenereDesc
                    },

                    Publisher = p.ProductPublisherNavigation == null ? null : new Publisher
                    {
                        PublisherId = p.ProductPublisherNavigation.PublisherId,
                        Name = p.ProductPublisherNavigation.Name,
                        Email = p.ProductPublisherNavigation.Email
                    },

                    ProductType = p.ProductTypeNavigation == null ? null : new ProductTypeMaster
                    {
                        TypeId = p.ProductTypeNavigation.TypeId,
                        TypeDesc = p.ProductTypeNavigation.TypeDesc
                    }
                })
                .FirstOrDefaultAsync();
        }





        public async Task<Product?> UpdateAsync(int id, Product Updatedproduct)
        {
            var existing = await _context.Products.FindAsync(id);
            if (existing == null)
                return null;

            _context.Entry(existing).CurrentValues.SetValues(Updatedproduct);
            await _context.SaveChangesAsync();
            return existing;
        }


        public async Task<IEnumerable<ProductResponseDto>> GetLibraryProductsAsync()
        {
            return await _context.Products
                .Where(p => p.IsLibrary.HasValue && p.IsLibrary.Value == 1UL)
                .Select(p => new ProductResponseDto
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    ProductImage = p.ProductImage,

                    ProductBaseprice = p.ProductBaseprice,
                    ProductOfferprice = p.ProductOfferprice,
                    DiscountPercent = p.DiscountPercent,

                    Rentable = p.IsRentable.HasValue && p.IsRentable.Value == 1UL,
                    RentPerDay = p.RentPerDay,
                    MinRentDays = p.MinRentDays,

                    Author = p.ProductAuthorNavigation != null
                        ? new Author
                        {
                            Name = p.ProductAuthorNavigation.Name
                        }
                        : null,

                    ProductType = p.ProductTypeNavigation != null
                        ? new ProductTypeMaster
                        {
                            TypeDesc = p.ProductTypeNavigation.TypeDesc
                        }
                        : null
                })
                .ToListAsync();
        }

        public List<Product> GetByLanguageDesc(string languageDesc)
        {
            return _context.Products
                .Include(p => p.ProductLangNavigation)
                .Where(p =>
                    p.ProductLangNavigation != null &&
                    p.ProductLangNavigation.LanguageDesc == languageDesc
                )
                .ToList();
        }

        public List<Product> GetByGenereDesc(string genereDesc)
        {
            return _context.Products
                .Include(p => p.ProductGenereNavigation)
                .Where(p =>
                    p.ProductGenereNavigation != null &&
                    p.ProductGenereNavigation.GenereDesc!.ToLower() == genereDesc.ToLower()
                )
                .ToList();
        }


        public async Task<IEnumerable<ProductResponseDto>> SearchByProductNameAsync(string name)
        {
            name = name.Trim().ToLower();

            return await _context.Products
                .Where(p => p.ProductName.ToLower().Contains(name))
                .Select(p => new ProductResponseDto
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    ProductImage = p.ProductImage,

                    ProductBaseprice = p.ProductBaseprice,
                    ProductOfferprice = p.ProductOfferprice,
                    DiscountPercent = p.DiscountPercent,

                    Rentable = p.IsRentable == 1,
                    IsLibrary = p.IsLibrary,
                    RentPerDay = p.RentPerDay,
                    MinRentDays = p.MinRentDays,

                    ProductIsbn = p.ProductIsbn,
                    ProductDescriptionLong = p.ProductDescriptionLong,

                    Author = p.ProductAuthorNavigation == null ? null : new Author
                    {
                        AuthorId = p.ProductAuthorNavigation.AuthorId,
                        Name = p.ProductAuthorNavigation.Name
                    },

                    Language = p.ProductLangNavigation == null ? null : new Language
                    {
                        LanguageId = p.ProductLangNavigation.LanguageId,
                        LanguageDesc = p.ProductLangNavigation.LanguageDesc
                    },

                    Genere = p.ProductGenereNavigation == null ? null : new Genere
                    {
                        GenereId = p.ProductGenereNavigation.GenereId,
                        GenereDesc = p.ProductGenereNavigation.GenereDesc
                    },

                    Publisher = p.ProductPublisherNavigation == null ? null : new Publisher
                    {
                        PublisherId = p.ProductPublisherNavigation.PublisherId,
                        Name = p.ProductPublisherNavigation.Name,
                        Email = p.ProductPublisherNavigation.Email
                    },

                    ProductType = p.ProductTypeNavigation == null ? null : new ProductTypeMaster
                    {
                        TypeId = p.ProductTypeNavigation.TypeId,
                        TypeDesc = p.ProductTypeNavigation.TypeDesc
                    }
                })
                .ToListAsync();
        }



        public async Task<IEnumerable<ProductResponseDto>> FilterAsync(string? genere, string? language)
        {
            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(genere))
            {
                query = query.Where(p =>
                    p.ProductGenereNavigation != null &&
                    p.ProductGenereNavigation.GenereDesc == genere
                );
            }

            if (!string.IsNullOrEmpty(language))
            {
                query = query.Where(p =>
                    p.ProductLangNavigation != null &&
                    p.ProductLangNavigation.LanguageDesc == language
                );
            }

            return await query
                .Select(p => new ProductResponseDto
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    ProductImage = p.ProductImage,

                    ProductBaseprice = p.ProductBaseprice,
                    ProductOfferprice = p.ProductOfferprice,
                    DiscountPercent = p.DiscountPercent,

                    Rentable = p.IsRentable == 1,
                    IsLibrary = p.IsLibrary,
                    RentPerDay = p.RentPerDay,
                    MinRentDays = p.MinRentDays,

                    ProductIsbn = p.ProductIsbn,
                    ProductDescriptionLong = p.ProductDescriptionLong,

                    Author = p.ProductAuthorNavigation == null ? null : new Author
                    {
                        AuthorId = p.ProductAuthorNavigation.AuthorId,
                        Name = p.ProductAuthorNavigation.Name
                    },

                    Language = p.ProductLangNavigation == null ? null : new Language
                    {
                        LanguageId = p.ProductLangNavigation.LanguageId,
                        LanguageDesc = p.ProductLangNavigation.LanguageDesc
                    },

                    Genere = p.ProductGenereNavigation == null ? null : new Genere
                    {
                        GenereId = p.ProductGenereNavigation.GenereId,
                        GenereDesc = p.ProductGenereNavigation.GenereDesc
                    },

                    Publisher = p.ProductPublisherNavigation == null ? null : new Publisher
                    {
                        PublisherId = p.ProductPublisherNavigation.PublisherId,
                        Name = p.ProductPublisherNavigation.Name,
                        Email = p.ProductPublisherNavigation.Email
                    },

                    ProductType = p.ProductTypeNavigation == null ? null : new ProductTypeMaster
                    {
                        TypeId = p.ProductTypeNavigation.TypeId,
                        TypeDesc = p.ProductTypeNavigation.TypeDesc
                    }
                })
                .ToListAsync();
        }


    }
}
