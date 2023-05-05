using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store_Core7.Model;
using Store_Core7.Payload;
using Store_Core7.Repository;
using Store_Core7.Utils;

namespace Store_Core7.Controllers
{
    [Route("/api/[controller]/[action]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IRepository<CategoryModel> _repository;
        private readonly IAuthService _authService;
        public CategoryController(IRepository<CategoryModel> repository, IAuthService authService)
        {
            _repository = repository;
            _authService = authService;
        }

        [HttpGet]
        [ActionName("GetAllCategories")]
        [Authorize(Roles = "Admin,User,Customer")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ActionResult<List<CategoryModel>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ActionResult<DefaultPayload>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ActionResult<DefaultPayload>))]
        public async Task<IActionResult> GetAllCategoriesAsync([FromHeader] string Authorization, [FromQuery] int page = 1, [FromQuery] int size = 10)
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
        [ActionName("GetCategory")]
        [Authorize(Roles = "Admin,User,Customer")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ActionResult<CategoryModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ActionResult<DefaultPayload>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ActionResult<DefaultPayload>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ActionResult<DefaultPayload>))]
        public async Task<IActionResult> GetCategory([FromHeader] string Authorization, [FromQuery] long id)
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
            catch(NotFoundException e)
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
        [ActionName("AddCategory")]
        [Authorize(Roles = "Admin,User")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IActionResult))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ActionResult<DefaultPayload>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ActionResult<DefaultPayload>))]
        public async Task<IActionResult> AddCategory([FromHeader] string Authorization, [FromBody] CategoryPayload category)
        {
            DefaultPayload response = new DefaultPayload();
            try
            {
                if (!_authService.IsTokenValid(Authorization))
                {
                    response.Message = "Invalid Token";
                    return Unauthorized(response);
                }

                CategoryModel categoryModel = new CategoryModel();
                categoryModel.CategoryName = category.CategoryName;
                categoryModel.IsActive = category.IsActive;

                var catId = await _repository.AddAsync(categoryModel);
                if (catId == 0)
                {
                    response.Message = "Error Inserting the Entity";
                    return BadRequest(response);
                }
                return Ok(new { CategoryId = catId });
            }
            catch (System.Exception ex)
            {
                response.Message = "Internal Error";
                return BadRequest(response);
            }
        }

        [HttpPut]
        [ActionName("UpdateCategory")]
        [Authorize(Roles = "Admin,User")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ActionResult<DefaultPayload>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ActionResult<DefaultPayload>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ActionResult<DefaultPayload>))]
        public async Task<IActionResult> UpdateCategory([FromHeader] string Authorization, [FromBody] CategoryPayload category, [FromQuery] long id)
        {
            DefaultPayload response = new DefaultPayload();
            try
            {
                if (!_authService.IsTokenValid(Authorization))
                {
                    response.Message = "Invalid Token";
                    return Unauthorized(response);
                }

                CategoryModel categoryModel = new CategoryModel();
                categoryModel.Id = id;
                categoryModel.CategoryName = category.CategoryName;
                categoryModel.IsActive = category.IsActive;

                var status = await _repository.UpdateAsync(categoryModel);
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
        [ActionName("DeleteCategory")]
        [Authorize(Roles = "Admin,User")]
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
