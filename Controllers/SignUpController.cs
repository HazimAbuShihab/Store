using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Store_Core7.Model;
using Store_Core7.Payload;
using Store_Core7.Utils;
using System.IdentityModel.Tokens.Jwt;

namespace Store_Core7.Controllers
{
    [Route("/api/Auth/[action]")]
    [ApiController]
    public class SignUpController : ControllerBase
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly IAuthService _authService;
        public SignUpController(UserManager<UserModel> userManager, IAuthService authService)
        {
            _userManager = userManager;
            _authService = authService;
        }
        [HttpPost]
        [AllowAnonymous]
        [ActionName("SignUp")]
        public async Task<IActionResult> SignUpAsync([FromBody] SignUpPayload requestMessage)
        {
            AuthResponse response = new AuthResponse();
            try
            {
                if (!ModelState.IsValid)
                {
                    response.Message = Constants.INVALID_MODEL;
                    return BadRequest(response);
                }

                //* Create a Super user
                if (await _userManager.FindByNameAsync("Superuser") is null)
                {
                    var superUser = new UserModel
                    {
                        Name = "Super User",
                        UserName = "Superuser",
                        Email = "Superuser@auth.com",
                        EmailConfirmed = true,
                        IsBlocked = false,
                        CreatedOn = DateTime.UtcNow,
                        Longitude = "",
                        Latitude = ""
                    };
                    var res = await _userManager.CreateAsync(superUser, "Su_per#User72");
                    await _userManager.AddToRoleAsync(superUser, "Admin");
                }

                //* Check if the email exist or not 
                if (await _userManager.FindByEmailAsync(requestMessage.Email) is not null)
                {
                    response.Message = Constants.EMAIL_EXIST;
                    return BadRequest(response);
                }

                //* Check if the username exist or not
                if (await _userManager.FindByNameAsync(requestMessage.UserName) is not null)
                {
                    response.Message = Constants.USERNAME_EXIST;
                    return BadRequest(response);
                }

                var user = new UserModel
                {
                    Name = requestMessage.Name,
                    UserName = requestMessage.UserName,
                    Email = requestMessage.Email,
                    IsBlocked = false,
                    CreatedOn = DateTime.UtcNow,
                    Longitude = requestMessage.Longitude,
                    Latitude = requestMessage.Latitude,
                    PhoneNumber = requestMessage.PhoneNumber
                };

                var result = await _userManager.CreateAsync(user, requestMessage.Password);

                if (!result.Succeeded)
                {
                    var errors = string.Empty;

                    foreach (var error in result.Errors)
                        errors += $"{error.Description},";
                    response.Message = errors;
                    return BadRequest(response);
                }

                await _userManager.AddToRoleAsync(user, "Customer");

                var jwtSecurityToken = await _authService.GenerateTokenAsync(user);

                response.Email = user.Email;
                response.ExpiresOn = jwtSecurityToken.ValidTo;
                response.IsAuthenticated = true;
                response.Roles = new List<string> { "Customer" };
                response.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                response.Username = user.UserName;

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Message = Constants.INTERNAL_ERORR;
                return BadRequest(response);
            }
        }
    }
}
