using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store_Core7.Model;
using Store_Core7.Payload;
using Store_Core7.Repository;
using Store_Core7.Utils;
using System.Data;

namespace Store_Core7.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SystemSettingController : ControllerBase
    {
        private readonly IRepository<SystemSettingModel> _repository;
        private readonly IAuthService _authService;
        public SystemSettingController(IRepository<SystemSettingModel> repository, IAuthService authService)
        {
            _repository = repository;
            _authService = authService;
        }

        [HttpGet]
        [ActionName("")]
        [Authorize(Roles = "Admin,User")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ActionResult<List<CategoryModel>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ActionResult<DefaultPayload>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ActionResult<DefaultPayload>))]
        public async Task<IActionResult> GetAllSystemSettings([FromHeader] string Authorization, [FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            DefaultPayload response = new DefaultPayload();
            try
            {
                if (!_authService.IsTokenValid(Authorization))
                {
                    response.Message = "Invalid Token";
                    return Unauthorized(response);
                }

                return Ok(await _repository.FindAllAsync(page, size));
            }
            catch (System.Exception)
            {
                response.Message = "Internal Error";
                return BadRequest(response);
            }
        }

        [HttpGet]
        [ActionName("GetById")]
        [Authorize(Roles = "Admin,User")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ActionResult<CategoryModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ActionResult<DefaultPayload>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ActionResult<DefaultPayload>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ActionResult<DefaultPayload>))]
        public async Task<IActionResult> GetSystemSetting([FromHeader] string Authorization, [FromQuery] long id)
        {
            DefaultPayload response = new DefaultPayload();
            try
            {
                if (!_authService.IsTokenValid(Authorization))
                {
                    response.Message = "Invalid Token";
                    return Unauthorized(response);
                }
                var category = await _repository.FindByIdAsync(id);
                return Ok(category);
            }
            catch (NotFoundException e)
            {
                response.Message = "The Entity doesn't Exist";
                return NotFound(response);
            }
            catch (System.Exception ex)
            {
                response.Message = "Internal Error";
                return BadRequest(response);
            }
        }

        [HttpPost]
        [ActionName("")]
        [Authorize(Roles = "Admin,User")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IActionResult))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ActionResult<DefaultPayload>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ActionResult<DefaultPayload>))]
        public async Task<IActionResult> AddSystemSetting([FromHeader] string Authorization, [FromBody] SystemSettingPayload systemSetting)
        {
            DefaultPayload response = new DefaultPayload();
            try
            {
                if (!_authService.IsTokenValid(Authorization))
                {
                    response.Message = "Invalid Token";
                    return Unauthorized(response);
                }

                SystemSettingModel systemSettingModel = new SystemSettingModel();
                systemSettingModel.Key = systemSetting.Key;
                systemSettingModel.Value = systemSetting.Value;
                systemSettingModel.IsActive = true;
                systemSettingModel.CreatedOn = DateTime.Now;




                var sysId = await _repository.AddAsync(systemSettingModel);
                if (sysId == 0)
                {
                    response.Message = "Error Inserting the Entity";
                    return BadRequest(response);
                }
                return Ok(new { SystemSettingId = sysId });
            }
            catch (System.Exception ex)
            {
                response.Message = "Internal Error";
                return BadRequest(response);
            }
        }

        [HttpPut]
        [ActionName("")]
        [Authorize(Roles = "Admin,User")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ActionResult<DefaultPayload>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ActionResult<DefaultPayload>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ActionResult<DefaultPayload>))]
        public async Task<IActionResult> UpdateCategory([FromHeader] string Authorization, [FromBody] SystemSettingPayload systemSetting, [FromQuery] long id)
        {
            DefaultPayload response = new DefaultPayload();
            try
            {
                if (!_authService.IsTokenValid(Authorization))
                {
                    response.Message = "Invalid Token";
                    return Unauthorized(response);
                }

                SystemSettingModel systemSettingModel = new SystemSettingModel();
                systemSettingModel.Id = id;
                systemSettingModel.Key = systemSetting.Key;
                systemSettingModel.Value = systemSetting.Value;
                systemSettingModel.IsActive = true;
                systemSettingModel.UpdatedOn = DateTime.Now;
                var status = await _repository.UpdateAsync(systemSettingModel);
                if (!status)
                {
                    response.Message = "Error Updating the Entity";
                    return BadRequest(response);
                }
                response.Message = "Updated Successfully";
                return Ok(response);
            }
            catch (System.Exception ex)
            {
                response.Message = "Internal Error";
                return BadRequest(response);
            }
        }

        [HttpDelete]
        [ActionName("")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ActionResult<DefaultPayload>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ActionResult<DefaultPayload>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ActionResult<DefaultPayload>))]
        public async Task<IActionResult> DeleteCategory([FromHeader] string Authorization, [FromQuery] long id)
        {
            DefaultPayload response = new DefaultPayload();
            try
            {
                if (!_authService.IsTokenValid(Authorization))
                {
                    response.Message = "Invalid Token";
                    return Unauthorized(response);
                }


                var status = await _repository.DeleteAsync(id);
                if (!status)
                {
                    response.Message = "Error Deleting the Entity";
                    return BadRequest(response);
                }
                response.Message = "Deleted Successfully";
                return Ok(response);
            }
            catch (System.Exception ex)
            {
                response.Message = "Internal Error";
                return BadRequest(response);
            }
        }
    }
}
