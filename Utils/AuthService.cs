using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Store_Core7.Model;
using Store_Core7.Payload;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Store_Core7.Utils
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JWT _jwt;
        public AuthService(UserManager<UserModel> userManager, RoleManager<IdentityRole> roleManager, IOptions<JWT> jwt)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwt = jwt.Value;
        }

        public async Task<JwtSecurityToken> GenerateTokenAsync(UserModel user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_jwt.ExpirationMinutes),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }

        public async Task<AuthResponse> GetTokenAsync(LoginPayload model)
        {
            var authModel = new AuthResponse();

            var user = await _userManager.FindByNameAsync(model.UserName);

            if (user is null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                authModel.Message = Constants.INCORRECT;
                return authModel;
            }

            var jwtSecurityToken = await GenerateTokenAsync(user);
            var rolesList = await _userManager.GetRolesAsync(user);

            authModel.IsAuthenticated = true;
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authModel.Email = user.Email;
            authModel.Username = user.UserName;
            authModel.ExpiresOn = jwtSecurityToken.ValidTo;
            authModel.Roles = rolesList.ToList();

            return authModel;
        }

        public bool IsTokenExpired(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);
            var expirationTime = jwtToken.ValidTo;
            var now = DateTime.UtcNow;
            if (expirationTime < now)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool IsTokenValid(string authHeader)
        {
            string token = null;
            if (authHeader != StringValues.Empty && authHeader.ToString().StartsWith("Bearer "))
            {
                token = authHeader.ToString().Substring("Bearer ".Length).Trim();
            }
            if (token == null)
            {
                return false;
            }
            if (IsTokenExpired(token))
            {
                return false;
            }
            return true;
        }
    }
}
