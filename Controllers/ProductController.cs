using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store_Core7.Model;
using Store_Core7.Payload;
using Store_Core7.Repository;
using Store_Core7.Utils;

namespace Store_Core7.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IRepository<ProductModel> _repository;
        private readonly IAuthService _authService;
        public ProductController(IRepository<ProductModel> repository, IAuthService authService)
        {
            _repository = repository;
            _authService = authService;
        }
        [HttpGet]
        [ActionName("")]
        [Authorize(Roles = "Admin,User,Customer")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ActionResult<List<CategoryModel>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ActionResult<DefaultPayload>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ActionResult<DefaultPayload>))]
        public async Task<IActionResult> GetAllProducts([FromHeader] string Authorization, [FromQuery] int page = 1, [FromQuery] int size = 10)
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
        [Authorize(Roles = "Admin,User,Customer")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ActionResult<CategoryModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ActionResult<DefaultPayload>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ActionResult<DefaultPayload>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ActionResult<DefaultPayload>))]
        public async Task<IActionResult> GetProduct([FromHeader] string Authorization, [FromQuery] long id)
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
                if (category == null)
                {
                    response.Message = "The Entity doesn't Exist";
                    return NotFound(response);
                }
                return Ok(category);
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
        public async Task<IActionResult> AddProduct([FromHeader] string Authorization, [FromBody] ProductPayload product)
        {
            DefaultPayload response = new DefaultPayload();
            try
            {
                if (!_authService.IsTokenValid(Authorization))
                {
                    response.Message = "Invalid Token";
                    return Unauthorized(response);
                }

                ProductModel productModel = new ProductModel();
                productModel.ProductName = product.ProductName;
                productModel.ProductBarCode = product.ProductBarCode;
                productModel.ProductPrice = product.ProductPrice;
                productModel.ProductImageName = product.ProductImageName;
                productModel.OldPrice = product.OldPrice;
                productModel.IsOffered = product.IsOffered;
                productModel.PercentageDiscount = product.PercentageDiscount;
                productModel.IsActive = product.IsActive;
                productModel.CategoryId = product.CategoryId;
                productModel.CreatedOn = DateTime.Now;

                var proId = await _repository.AddAsync(productModel);
                if (proId == 0)
                {
                    response.Message = "Error Inserting the Entity";
                    return BadRequest(response);
                }
                return Ok(new { ProductId = proId });
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
        public async Task<IActionResult> UpdateCategory([FromHeader] string Authorization, [FromBody] ProductPayload product, [FromQuery] long id)
        {
            DefaultPayload response = new DefaultPayload();
            try
            {
                if (!_authService.IsTokenValid(Authorization))
                {
                    response.Message = "Invalid Token";
                    return Unauthorized(response);
                }

                ProductModel productModel = new ProductModel();
                productModel.Id = id;
                productModel.ProductName = product.ProductName;
                productModel.ProductBarCode = product.ProductBarCode;
                productModel.ProductPrice = product.ProductPrice;
                productModel.ProductImageName = product.ProductImageName;
                productModel.OldPrice = product.OldPrice;
                productModel.IsOffered = product.IsOffered;
                productModel.PercentageDiscount = product.PercentageDiscount;
                productModel.IsActive = product.IsActive;

                var status = await _repository.UpdateAsync(productModel);
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
