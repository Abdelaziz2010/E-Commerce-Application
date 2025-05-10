using Ecom.Application.Services.Interfaces;
using Ecom.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("Create-Or-Update-Payment")]
        public async Task<ActionResult<Cart>> CreateOrUpdate(string cartId, int? deliveryMethodId)
        {
            return await _paymentService.CreateOrUpdatePaymentAsync(cartId, deliveryMethodId);
        }

    }
}
