namespace BookWormNET.dto
{
    public class ProductBeneficiarydto
    {
        public int ProdbenId { get; set; }
        public string BeneficiaryName { get; set; }
        public string ProductName { get; set; }

        public decimal RoyaltyReceived { get; set; }
        public decimal? RoyaltyPercent { get; set; }
        public decimal? TotalRoyalty { get; set; }
    }
}
