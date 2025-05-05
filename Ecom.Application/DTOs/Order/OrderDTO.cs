
namespace Ecom.Application.DTOs.Order
{
    public record OrderDTO
    {
        public int DeliveryMethodId { get; set; }
        public string CartId { get; set; }
        public ShippingAddressDTO ShippingAddress { get; set; }
    }
}
