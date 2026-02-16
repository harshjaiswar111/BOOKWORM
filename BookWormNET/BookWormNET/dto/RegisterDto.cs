using System.Text.Json.Serialization;

namespace BookWormNET.dto
{
    public class RegisterDto
    {
        [JsonPropertyName("name")]
        public string UserName { get; set; }

        [JsonPropertyName("email")]
        public string UserEmail { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }

        [JsonPropertyName("phone")]
        public string? UserPhone { get; set; }

        [JsonPropertyName("address")]
        public string? UserAddress { get; set; }

        // frontend sends this but you ignore it safely
        [JsonPropertyName("admin")]
        public bool Admin { get; set; }
    }
}
