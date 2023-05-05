using System.Text.Json.Serialization;

namespace Store_Core7.Payload
{
    public class ProductPayload
    {
        [JsonPropertyName("ProductName")]
        public string? ProductName { get; set; }
        [JsonPropertyName("ProductBarCode")]
        public string? ProductBarCode { get; set; }
        [JsonPropertyName("ProductPrice")]
        public decimal ProductPrice { get; set; }
        [JsonPropertyName("ProductImageName")]
        public string? ProductImageName { get; set; }
        [JsonPropertyName("OldPrice")]
        public Nullable<decimal> OldPrice { get; set; }
        [JsonPropertyName("IsOffered")]
        public Nullable<bool> IsOffered { get; set; }
        [JsonPropertyName("PercentageDiscount")]
        public Nullable<int> PercentageDiscount { get; set; }
        [JsonPropertyName("IsActive")]
        public bool IsActive { get; set; }
        [JsonPropertyName("CategoryId")]
        public long CategoryId { get; set; }
    }
}
