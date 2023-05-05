using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Store_Core7.Payload
{
    public class SystemSettingPayload
    {
        [JsonPropertyName("Key")]
        [Required]
        public string? Key { get; set; }
        [JsonPropertyName("Value")]
        [Required]
        public string? Value { get; set; }
    }
}
