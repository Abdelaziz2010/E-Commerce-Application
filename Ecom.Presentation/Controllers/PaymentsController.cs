using Ecom.Application.Services.Interfaces;
using Ecom.Domain.Entities;
using Ecom.Domain.Entities.Orders;
using Ecom.Presentation.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;

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

        const string endpointSecret = "whsec_28cc3dec50be3eaba23c0d5217e31f075148d84948bb1e7aa84452952a3a9461";

        [HttpPost("WebHook")]
        public async Task<IActionResult> UpdateOrderStatusWithStripe()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json, 
                    Request.Headers["Stripe-Signature"], endpointSecret, throwOnApiVersionMismatch: false);

                PaymentIntent paymentIntent;

                Order order;

                // Handle the event
                if(stripeEvent.Type == Events.PaymentIntentSucceeded)
                {
                    paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                    order = await _paymentService.UpdateOrderStatusToSuccess(paymentIntent.Id);
                }
                else if(stripeEvent.Type == Events.PaymentIntentPaymentFailed)
                {
                    paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                    order = await _paymentService.UpdateOrderStatusToFailed(paymentIntent.Id);
                }
                // ... handle other event types
                else
                {
                    Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseAPI(400, ex.Message));
            }
        }
    }
}
