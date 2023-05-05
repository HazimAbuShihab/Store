using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Store_Core7.Payload
{
    public class LoginPayload
    {
        [JsonPropertyName("UserName")]
        [Required]
        public string? UserName { get; set; }
        [JsonPropertyName("Password")]
        [Required]
        public string? Password { get; set; }
    }
}
