using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store_Core7.Model;
using Store_Core7.Payload;
using Store_Core7.Utils;

namespace Store_Core7.Controllers
{
    [Route("api/Auth/Role/[action]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _rolemanager;
        private readonly UserManager<UserModel> _userManager;
        private readonly IAuthService _authService;
        public RoleController(RoleManager<IdentityRole> rolemanager, UserManager<UserModel> userManager, IAuthService authService)
        {
            _rolemanager = rolemanager;
            _userManager = userManager;
            _authService = authService;
        }

        [HttpGet]
        [ActionName("")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetAllRole([FromHeader] string Authorization, [FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            try
            {
                if (!_authService.IsTokenValid(Authorization))
                {
                    DefaultPayload response = new DefaultPayload();
                    response.Message = "Invalid Token";
                    return Unauthorized(response);
                }
                var roles = await _rolemanager.Roles.Skip((page - 1) * size).Take(size).ToListAsync();
                return Ok(roles);
            }
            catch (System.Exception ex)
            {
                DefaultPayload response = new DefaultPayload();
                response.Message = "Internal Error";
                return BadRequest(response);
            }
        }

        [HttpGet]
        [ActionName("GetById")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetRole([FromHeader] string Authorization, [FromQuery] string id)
        {
            try
            {
                if (!_authService.IsTokenValid(Authorization))
                {
                    DefaultPayload response = new DefaultPayload();
                    response.Message = "Invalid Token";
                    return Unauthorized(response);
                }
                var role = await _rolemanager.FindByIdAsync(id);
                if (role == null)
                {
                    DefaultPayload response = new DefaultPayload();
                    response.Message = "Role doesn't Exist";
                    return NotFound(response);
                }
                return Ok(role);
            }
            catch (System.Exception)
            {
                DefaultPayload response = new DefaultPayload();
                response.Message = "Internal Error";
                return BadRequest(response);
            }
        }

        [HttpGet]
        [ActionName("GetByUserId")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetUserRoles([FromHeader] string Authorization, [FromQuery] string userId)
        {
            try
            {
                if (!_authService.IsTokenValid(Authorization))
                {
                    DefaultPayload response = new DefaultPayload();
                    response.Message = "Invalid Token";
                    return Unauthorized(response);
                }
                var user = await _userManager.FindByIdAsync(userId);
                var roles = await _userManager.GetRolesAsync(user);
                return Ok(roles);
            }
            catch (System.Exception ex)
            {
                DefaultPayload response = new DefaultPayload();
                response.Message = "Internal Error";
                return BadRequest(response);
            }
        }

        [HttpPut]
        [ActionName("")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateRole([FromHeader] string Authorization, [FromQuery] string id, [FromQuery] string roleNewName)
        {
            DefaultPayload response = new DefaultPayload();
            try
            {
                if (!_authService.IsTokenValid(Authorization))
                {
                    response.Message = "Invalid Token";
                    return Unauthorized(response);
                }
                var role = await _rolemanager.FindByIdAsync(id);
                if (role == null)
                {
                    response.Message = "Role doesn't Exist";
                    return NotFound(response);
                }

                role.Name = roleNewName;
                var result = await _rolemanager.UpdateAsync(role);
                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }
                response.Message = "Updated Successfully";
                return Ok(response);
            }
            catch (System.Exception)
            {
                response.Message = "Internal Error";
                return BadRequest(response);
            }
        }


        [HttpPost]
        [ActionName("AddUserRole")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddUserRole([FromHeader] string Authorization, [FromBody] RolePayload requestMessage)
        {
            DefaultPayload response = new DefaultPayload();
            try
            {
                if (!_authService.IsTokenValid(Authorization))
                {
                    response.Message = "Invalid Token";
                    return Unauthorized(response);
                }
                var user = await _userManager.FindByNameAsync(requestMessage.UserName);
                if (user == null)
                {
                    response.Message = "User Not Found";
                    return NotFound(response);
                }

                var role = await _rolemanager.FindByNameAsync(requestMessage.RoleName);
                if (role == null)
                {
                    response.Message = "Role Not Found";
                    return NotFound(response);
                }

                var result = await _userManager.AddToRoleAsync(user, requestMessage.RoleName);
                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }
                response.Message = "Role Added Successfully";
                return Ok(response);
            }
            catch (System.Exception)
            {
                response.Message = "Internal Error";
                return BadRequest(response);
            }
        }

        [HttpDelete]
        [ActionName("RemoveRoleFromUser")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveRoleFromUser([FromHeader] string Authorization, [FromBody] RolePayload requestMessage)
        {
            DefaultPayload response = new DefaultPayload();
            try
            {
                if (!_authService.IsTokenValid(Authorization))
                {
                    response.Message = "Invalid Token";
                    return Unauthorized(response);
                }
                var user = await _userManager.FindByNameAsync(requestMessage.UserName);
                if (user == null)
                {
                    response.Message = "User Not Found";
                    return NotFound(response);
                }
                var result = await _userManager.RemoveFromRoleAsync(user, requestMessage.RoleName);
                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }
                response.Message = "Removed Successfully";
                return Ok(response);
            }
            catch (System.Exception)
            {
                response.Message = "Internal Error";
                return BadRequest(response);
            }
        }
    }
}
