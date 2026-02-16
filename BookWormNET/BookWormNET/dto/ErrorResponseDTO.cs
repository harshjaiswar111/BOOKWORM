namespace BookWormNET.DTO
{
    public class ErrorResponseDTO
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? Details { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
