using System.Text.Json.Serialization;

namespace Store_Core7.Payload
{
    public class DefaultPayload
    {
        [JsonPropertyName("Message")]
        public string? Message { get; set; }
    }
}
