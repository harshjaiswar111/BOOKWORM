using BookWormNET.Models;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BookWormNET.dto
{
    public class CheckoutRequestDto
    {
        //public int UserId { get; set; }
        public List<int> ProductIds { get; set; } = new();

        [JsonPropertyName("rentDays")]
        public int? RentDays { get; set; }
        public TransactionType TransactionType { get; set; }
    }
}
