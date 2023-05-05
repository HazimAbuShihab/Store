using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Store_Core7.Payload
{
    public class RolePayload
    {
        [JsonPropertyName("UserName")]
        [Required]
        public string? UserName { get; set; }
        [JsonPropertyName("RoleName")]
        [Required]
        public string? RoleName { get; set; }
    }
}
