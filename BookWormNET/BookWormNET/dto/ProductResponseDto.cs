using BookWormNET.Models;

namespace BookWormNET.dto
{
    
        public class ProductResponseDto
        {
            public int ProductId { get; set; }
            public string ProductName { get; set; } = null!;
            public string? ProductImage { get; set; }

            public decimal ProductBaseprice { get; set; }
            public decimal? ProductOfferprice { get; set; }
            public decimal? DiscountPercent { get; set; }

            // 👇 frontend expects this name
            public bool Rentable { get; set; }
            public ulong? IsLibrary { get; set; }
            public decimal? RentPerDay { get; set; }
            public int? MinRentDays { get; set; }

        public string? ProductIsbn { get; set; }
        public string? ProductDescriptionLong { get; set; }

        // 👇 nested objects
        public Author? Author { get; set; }
            public ProductTypeMaster? ProductType { get; set; }

        public Language? Language { get; set; }
        public Genere? Genere { get; set; }
        public Publisher? Publisher { get; set; }

    }

    //    public class AuthorDto
    //    {
    //        public string? Name { get; set; }
    //    }

    //    public class ProductTypeDto
    //    {
    //        public string? TypeDesc { get; set; }
    //    }

    //public class LanguageDto
    //{
    //    public string? LanguageDesc { get; set; }
    //}

    //public class GenereDto
    //{
    //    public string? GenereDesc { get; set; }
    //}

    //public class PublisherDto
    //{
    //    public string? Name { get; set; }
    //    public string? Email { get; set; }
    //}


}
