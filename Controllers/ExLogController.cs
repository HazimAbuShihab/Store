using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store_Core7.Model;
using Store_Core7.Payload;
using Store_Core7.Repository;
using Store_Core7.Utils;

namespace Store_Core7.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ExLogController : ControllerBase
    {
        private readonly IRepository<LogModel> _repository;
        private readonly IAuthService _authService;
        public ExLogController(IRepository<LogModel> repository, IAuthService authService)
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
        public async Task<IActionResult> GetAllLogs([FromHeader] string Authorization, [FromQuery] int page = 1, [FromQuery] int size = 10)
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
        public async Task<IActionResult> GetLog([FromHeader] string Authorization, [FromQuery] long id)
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

    }
}
