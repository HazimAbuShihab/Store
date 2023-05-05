using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Store_Core7.Model;
using Store_Core7.Payload;
using Store_Core7.Utils;

namespace Store_Core7.Controllers
{
    [Route("/api/Auth/[action]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly IAuthService _authService;

        public LoginController(UserManager<UserModel> userManager, IAuthService authService)
        {
            _userManager = userManager;
            _authService = authService;
        }

        [HttpPost]
        [AllowAnonymous]
        [ActionName("Login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ActionResult<List<AuthResponse>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ActionResult<AuthResponse>))]
        public async Task<IActionResult> LoginAsync([FromBody] LoginPayload requestMessage)
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

                response = await _authService.GetTokenAsync(requestMessage);
                if (string.IsNullOrEmpty(response.Username))
                {
                    return BadRequest(response);
                }
                return Ok(response);
            }
            catch (System.Exception ex)
            {
                response.Message = Constants.INTERNAL_ERORR;
                return BadRequest(response);
            }

        }
    }
}
