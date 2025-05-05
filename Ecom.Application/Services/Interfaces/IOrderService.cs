using Ecom.Application.DTOs.Order;
using Ecom.Domain.Entities.Orders;
namespace Ecom.Application.Services.Interfaces
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(OrderDTO order, string buyerEmail);
        Task<IReadOnlyList<Order>> GetAllOrdersForUserAsync(string buyerEmail); 
        Task<Order> GetOrderByIdAsync(int id, string buyerEmail);
        Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodAsync();
    }
}
