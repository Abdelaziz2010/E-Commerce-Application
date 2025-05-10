
using Ecom.Application.Interfaces.Repositories;
using Ecom.Application.Services.Interfaces;
using Ecom.Domain.Entities;
using Ecom.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Stripe;

namespace Ecom.Infrastructure.Implementation.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _work;
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;
        public PaymentService(IUnitOfWork work, IConfiguration configuration, AppDbContext context)
        {
            _work = work;
            _configuration = configuration;
            _context = context;
        }
        public async Task<Cart> CreateOrUpdatePaymentAsync(string cartId, int? deliveryMethodId)
        {
            var cart = await _work.CartRepository.GetCartAsync(cartId);

            StripeConfiguration.ApiKey = _configuration["StripeSettings:SecretKey"];

            decimal shippingPrice = 0m;

            if (deliveryMethodId.HasValue)
            {
                var deliveryMethod = await _context.DeliveryMethods.AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == deliveryMethodId.Value);

                shippingPrice = deliveryMethod.Price;
            }

            foreach (var item in cart.CartItems)
            {
                var product = await _work.ProductRepository.GetByIdAsync(item.Id);
                
                item.Price = product.NewPrice;
            }


            PaymentIntentService paymentIntentService = new PaymentIntentService();

            PaymentIntent _paymentIntent;

            
            if (string.IsNullOrEmpty(cart.PaymentIntentId))        // create a new PaymentIntent
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long)cart.CartItems.Sum(x => x.Quantity * (x.Price * 100)) + (long)(shippingPrice * 100),
                    Currency = "USD",
                    PaymentMethodTypes = new List<string> { "card" }
                };

                _paymentIntent = await paymentIntentService.CreateAsync(options);

                cart.PaymentIntentId = _paymentIntent.Id;

                cart.ClientSecret = _paymentIntent.ClientSecret;
            }
            else                                                    //update the existing PaymentIntent
            {
                var options = new PaymentIntentUpdateOptions
                {
                    Amount = (long)cart.CartItems.Sum(x => x.Quantity * (x.Price * 100)) + (long)(shippingPrice * 100)
                };

                await paymentIntentService.UpdateAsync(cart.PaymentIntentId, options); 
            }

            await _work.CartRepository.UpdateOrCreateCartAsync(cart);

            return cart;
        }
    }
}
