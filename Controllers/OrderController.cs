using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store_Core7.Model;
using Store_Core7.Payload;
using Store_Core7.Repository;
using Store_Core7.Utils;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Data;

namespace Store_Core7.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IRepository<OrderModel> _repository;
        private readonly IAuthService _authService;
        private readonly AppDBContext _dbContext;
        public OrderController(IRepository<OrderModel> repository, IAuthService authService, AppDBContext dbContext)
        {
            _repository = repository;
            _authService = authService;
            _dbContext = dbContext;
        }
        [HttpGet]
        [ActionName("GetAllOrders")]
        [Authorize(Roles = "Admin,User,Customer")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ActionResult<List<CategoryModel>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ActionResult<DefaultPayload>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ActionResult<DefaultPayload>))]
        public IActionResult GetAllUserOrders([FromHeader] string Authorization, [FromQuery] string userId)
        {
            DefaultPayload response = new DefaultPayload();
            try
            {
                if (!_authService.IsTokenValid(Authorization))
                {
                    response.Message = "Invalid Token";
                    return Unauthorized(response);
                }
                var result = _dbContext.Orders.Where(o => o.UserId == userId).ToList();
                return Ok(result);
            }
            catch (System.Exception)
            {
                response.Message = "Internal Error";
                return BadRequest(response);
            }
        }

        [HttpGet]
        [ActionName("GetOrder")]
        [Authorize(Roles = "Admin,User,Customer")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ActionResult<CategoryModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ActionResult<DefaultPayload>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ActionResult<DefaultPayload>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ActionResult<DefaultPayload>))]
        public IActionResult GetOrder([FromHeader] string Authorization, [FromQuery] string guid)
        {
            DefaultPayload response = new DefaultPayload();
            try
            {
                if (!_authService.IsTokenValid(Authorization))
                {
                    response.Message = "Invalid Token";
                    return Unauthorized(response);
                }
                var orderDetails = _dbContext.Orders.Where(o => o.GUID == guid).ToList();
                return Ok(orderDetails);
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
        [ActionName("AddOrder")]
        [Authorize(Roles = "Admin,User,Customer")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IActionResult))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ActionResult<DefaultPayload>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ActionResult<DefaultPayload>))]
        public async Task<IActionResult> AddOrder([FromHeader] string Authorization, [FromBody] OrderPayload order)
        {
            DefaultPayload response = new DefaultPayload();
            try
            {
                if (!_authService.IsTokenValid(Authorization))
                {
                    response.Message = "Invalid Token";
                    return Unauthorized(response);
                }
                string GUID = Guid.NewGuid().ToString();
                foreach (var item in order.products)
                {
                    OrderModel orderModel = new OrderModel
                    {
                        GUID = GUID,
                        IsDone = false,
                        Price = item.ProductPrice,
                        NewLatitude = order.NewLatitude,
                        NewLongitude = order.NewLongitude,
                        UserId = order.UserId,
                        ProductId = item.ProductId
                    };
                    var orderId = await _repository.AddAsync(orderModel);
                    if (orderId == 0)
                    {
                        response.Message = "Error Inserting the Entity";
                        return BadRequest(response);
                    }
                }
                return Ok(new { OrderGUID = GUID });
            }
            catch (System.Exception ex)
            {
                response.Message = "Internal Error";
                return BadRequest(response);
            }
        }
    }
}
