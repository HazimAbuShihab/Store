using Store_Core7.Model;
using Store_Core7.Payload;
using System.IdentityModel.Tokens.Jwt;

namespace Store_Core7.Utils
{
    public interface IAuthService
    {
        Task<JwtSecurityToken> GenerateTokenAsync(UserModel user);
        Task<AuthResponse> GetTokenAsync(LoginPayload model);
        bool IsTokenExpired(string token);
        bool IsTokenValid(string authHeader);
    }
}
