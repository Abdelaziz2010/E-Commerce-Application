namespace Ecom.Application.DTOs.Order
{
    public record OrderItemDTO
    {
        public int ProductItemId { get; set; }
        public string MainImage { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
