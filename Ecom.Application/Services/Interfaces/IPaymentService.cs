
using Ecom.Domain.Entities;
using Ecom.Domain.Entities.Orders;

namespace Ecom.Application.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<Cart> CreateOrUpdatePaymentAsync(string cartId, int? deliveryMethodId);
        Task<Order> UpdateOrderStatusToSuccess(string paymentIntentId);
        Task<Order> UpdateOrderStatusToFailed(string paymentIntentId);
    }
}
