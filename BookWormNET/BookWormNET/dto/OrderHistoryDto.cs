namespace BookWormNET.dto
{
    public class OrderHistoryDto
    {
        public long TransactionId { get; set; }
        public string ProductName { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal Amount { get; set; }
    }
}