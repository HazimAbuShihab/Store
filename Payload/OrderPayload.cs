using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Store_Core7.Payload
{
    public class OrderPayload
    {
        [JsonPropertyName("UserId")]
        public string? UserId { get; set; }
        [Column("NewLatitude")]
        public string? NewLatitude { get; set; }
        [Column("NewLongitude")]
        public string? NewLongitude { get; set; }
        [JsonPropertyName("Products")]
        public List<ProductOrder>? products { get; set; }
    }
    public class ProductOrder
    {
        [JsonPropertyName("ProductId")]
        public long ProductId { get; set; }
        [JsonPropertyName("ProductPrice")]
        public decimal ProductPrice { get; set; }
    }
}
