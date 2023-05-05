using System.Text.Json.Serialization;

namespace Store_Core7.Payload
{
    public class SignUpPayload
    {
        [JsonPropertyName("Name")]
        public string? Name { get; set; }
        [JsonPropertyName("UserName")]
        public string? UserName { get; set; }
        [JsonPropertyName("Password")]
        public string? Password { get; set; }
        [JsonPropertyName("Email")]
        public string? Email { get; set; }
        [JsonPropertyName("PhoneNumber")]
        public string? PhoneNumber { get; set; }
        [JsonPropertyName("Latitude")]
        public string? Latitude { get; set; }
        [JsonPropertyName("Longitude")]
        public string? Longitude { get; set; }
    }
}
