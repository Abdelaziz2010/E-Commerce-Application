
namespace Ecom.Domain.Entities.Orders
{
    public class Order : BaseEntity<int>
    {
        public Order() { }
        
        public Order(string buyerEmail, decimal subTotal, ShippingAddress shippingAddress, DeliveryMethod deliveryMethod, 
                     IReadOnlyList<OrderItem> orderItems)
        {
            BuyerEmail = buyerEmail;
            SubTotal = subTotal;
            ShippingAddress = shippingAddress;
            DeliveryMethod = deliveryMethod;
            OrderItems = orderItems;
        }
        public string BuyerEmail { get; set; }
        public decimal SubTotal { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public ShippingAddress ShippingAddress { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; } 
        public IReadOnlyList<OrderItem> OrderItems { get; set;}
        public Status Status { get; set; } = Status.Pending;   // Default status is Pending
        public decimal Total => SubTotal + DeliveryMethod.Price;
       
        //public decimal GetTotal()            
        //{
        //    return SubTotal + DeliveryMethod.Price;
        //}
    }
}
