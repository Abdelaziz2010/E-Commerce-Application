using Ecom.Application.DTOs.Order;
using Ecom.Domain.Entities.Orders;
namespace Ecom.Application.Services.Interfaces
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(OrderDTO order, string buyerEmail);
        Task<IReadOnlyList<OrderToReturnDTO>> GetAllOrdersForUserAsync(string buyerEmail); 
        Task<OrderToReturnDTO> GetOrderByIdAsync(int id, string buyerEmail);
        Task<IReadOnlyList<DeliveryMethodDTO>> GetDeliveryMethodsAsync();
    }
}
