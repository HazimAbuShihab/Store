using System.Text.Json.Serialization;

namespace Store_Core7.Payload
{
    public class CategoryPayload
    {
        [JsonPropertyName("CategoryName")]
        public string? CategoryName { get; set; }
        [JsonPropertyName("IsActive")]
        public bool IsActive { get; set; }
    }
}
