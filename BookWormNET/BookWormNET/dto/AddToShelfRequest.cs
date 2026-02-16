namespace BookWormNET.dto
{
    public class AddToShelfRequest
    {
        public int ProductId { get; set; }
        public int? RentDays { get; set; }
    }
}