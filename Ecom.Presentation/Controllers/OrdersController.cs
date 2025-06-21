using Ecom.Application.DTOs.Order;
using Ecom.Application.Services.Interfaces;
using Ecom.Presentation.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;

namespace Ecom.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [EnableRateLimiting("WritePolicy")]
        [HttpPost("Create-Order")]
        public async Task<IActionResult> CreateOrder([FromBody] OrderDTO orderDTO)
        {
            if (orderDTO == null)
            {
                return BadRequest(new ResponseAPI(400, "Order data is null"));
            }

            var buyerEmail = User.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(buyerEmail))
            {
                return Unauthorized(new ResponseAPI(401));
            }

            var order = await _orderService.CreateOrderAsync(orderDTO, buyerEmail);

            return Ok(order);
        }

        [EnableRateLimiting("ReadOnlyPolicy")]
        [HttpGet("Get-All-Orders-For-User")]
        public async Task<IActionResult> GetAllOrdersForUser()
        {
            var buyerEmail = User.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(buyerEmail))
            {
                return Unauthorized(new ResponseAPI(401));
            }

            var orders = await _orderService.GetAllOrdersForUserAsync(buyerEmail);
         
            if (orders == null || orders.Count == 0)
            {
                return NotFound(new ResponseAPI(404, "No orders found for this user"));
            }
            
            return Ok(orders);
        }

        [HttpGet("Get-Order-By-Id/{id}")]
        public async Task<IActionResult> GetOrderById(int id) 
        {
            var buyerEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            
            if (string.IsNullOrEmpty(buyerEmail))
            {
                return Unauthorized(new ResponseAPI(401));
            }
            
            var order = await _orderService.GetOrderByIdAsync(id, buyerEmail);
            
            if (order == null)
            {
                return NotFound(new ResponseAPI(404, "Order not found"));
            }
            
            return Ok(order);
        }

        [HttpGet("Get-Delivery-Methods")]
        public async Task<IActionResult> GetDeliveryMethods()
        {
            var deliveryMethods = await _orderService.GetDeliveryMethodsAsync();

            if (deliveryMethods == null || deliveryMethods.Count == 0)
            {
                return NotFound(new ResponseAPI(404, "No delivery methods found"));
            }

            return Ok(deliveryMethods);
        }
    }
}
