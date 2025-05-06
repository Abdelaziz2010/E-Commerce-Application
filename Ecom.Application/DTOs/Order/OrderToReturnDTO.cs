
using Ecom.Domain.Entities.Orders;

namespace Ecom.Application.DTOs.Order
{
    public record OrderToReturnDTO
    {
        public int Id { get; set; }
        public string BuyerEmail { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }
        public DateTime OrderDate { get; set; }
        public ShippingAddress ShippingAddress { get; set; }
        public string DeliveryMethod { get; set; }
        public IReadOnlyList<OrderItemDTO> OrderItems { get; set; }
        public string Status { get; set; } 
    }
}
