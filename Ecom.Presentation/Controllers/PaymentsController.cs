using Ecom.Application.Services.Interfaces;
using Ecom.Domain.Entities;
using Ecom.Domain.Entities.Orders;
using Ecom.Presentation.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Stripe;

namespace Ecom.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;

        public PaymentsController(IPaymentService paymentService, ILogger logger, IConfiguration configuration)
        {
            _paymentService = paymentService;
            _logger = logger;
            _configuration = configuration;
        }

        [EnableRateLimiting("WritePolicy")]
        [HttpPost("Create-Or-Update-Payment")]
        public async Task<ActionResult<Cart>> CreateOrUpdate(string cartId, int? deliveryMethodId)
        {
            return await _paymentService.CreateOrUpdatePaymentAsync(cartId, deliveryMethodId);
        }

        /// <summary>
        /// Why Webhooks?
        /// No manual polling: The app doesn’t have to constantly ask Stripe "Did the payment go through?".
        /// Real-time updates: Immediate order status updates for better user experience and backend accuracy.
        /// </summary> 

        [HttpPost("WebHook")]       // https://Ecom.com/api/Payments/WebHook
        public async Task<IActionResult> UpdateOrderStatusWithStripe()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            var endpointSecret = _configuration["StripeSettings:WebhookSecret"];

            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json, 
                    Request.Headers["Stripe-Signature"], endpointSecret, throwOnApiVersionMismatch: false);

                PaymentIntent paymentIntent;

                Order order;

                switch(stripeEvent.Type)      // Handle the event
                {
                    case EventTypes.PaymentIntentSucceeded:
                        paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                        order = await _paymentService.UpdateOrderStatusToSuccess(paymentIntent.Id);
                        break;
                    case EventTypes.PaymentIntentPaymentFailed:
                        paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                        order = await _paymentService.UpdateOrderStatusToFailed(paymentIntent.Id);
                        break;
                    default:   
                        _logger.LogInformation("Unhandled event type: {EventType}", stripeEvent.Type);
                        break;
                }

                return Ok();
            }
            catch (StripeException ex)
            {
                _logger.LogError(ex, $"Stripe webhook error: {ex.Message}");
                return BadRequest(new ResponseAPI(400, $"Stripe webhook error: {ex.Message}"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Webhook error: {ex.Message}");
                return BadRequest(new ResponseAPI(400, ex.Message));
            }
        }
    }
}
