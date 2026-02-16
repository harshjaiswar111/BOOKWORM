namespace BookWormNET.dto
{
    public class AddToCart
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        //int UserId { get; set; }
        public int Qty { get; set; }

    }
}
