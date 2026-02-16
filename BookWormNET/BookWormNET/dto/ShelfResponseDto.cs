namespace BookWormNET.dto
{
    //public class ShelfResponseDto
    //{
    //    public int ShelfId { get; set; }
    //    public DateTime? ProductExpiryDate { get; set; }
    //    public string PurchaseType { get; set; } = null!;
    //    public ShelfProductDto Product { get; set; } = null!;
    //}


    public class ShelfResponseDto
    {
        public int ShelfId { get; set; }

        public DateTime? ProductExpiryDate { get; set; }

        // ✅ existing (keep)
        public string PurchaseType { get; set; } = null!;

        // ✅ ADD THIS (backend alias for frontend)
        public string Type { get; set; } = null!;

        public ShelfProductDto Product { get; set; } = null!;
    }

    public class ShelfProductDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public string? ProductImage { get; set; }

        public ShelfAuthorDto? Author { get; set; }
    }

    public class ShelfAuthorDto
    {
        public string? Name { get; set; }
    }
}