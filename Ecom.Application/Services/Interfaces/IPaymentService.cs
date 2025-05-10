
using Ecom.Domain.Entities;

namespace Ecom.Application.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<Cart> CreateOrUpdatePaymentAsync(string cartId, int? deliveryMethodId);
    }
}
