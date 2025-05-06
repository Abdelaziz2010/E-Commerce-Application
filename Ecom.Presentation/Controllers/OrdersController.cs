using Ecom.Application.DTOs.Order;
using Ecom.Application.Services.Interfaces;
using Ecom.Presentation.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        [HttpPost("Create-Order")]
        public async Task<IActionResult> CreateOrder([FromBody] OrderDTO orderDTO)
        {
            if (orderDTO == null)
            {
                return BadRequest(new ResponseAPI(400, "Order data is null"));
            }

            var buyerEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            //var buyerEmail = "islam@gmail.com"; // For testing purposes, hardcoded email

            if (string.IsNullOrEmpty(buyerEmail))
            {
                return Unauthorized(new ResponseAPI(401));
            }

            var order = await _orderService.CreateOrderAsync(orderDTO, buyerEmail);

            return Ok(order);
        }
    }
}
