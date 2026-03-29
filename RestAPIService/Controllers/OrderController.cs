using Microsoft.AspNetCore.Mvc;
using RestAPIService.Application.IServices;
using RestAPIService.Application.ViewModels.OrderModel;
using RestAPIService.Application.ViewModels.ProductModel;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RestAPIService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [HttpGet("getOrderByUserId")]
        public async Task<IActionResult> GetOrder(int userId)
        {
            var result = await _orderService.GetOrderByUserId(userId);
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpPost("AddOrder")]
        public async Task<IActionResult> AddOrder(CreateOrderRequest createOrderRequest)
        {
            var result = await _orderService.AddOrder(createOrderRequest);
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpPut("UpdateOrder")]
        public async Task<IActionResult> UpdateOrder(UpdateOrderRequest update)
        {
            var result = await _orderService.UpdateOrder(update);
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
    }
}
