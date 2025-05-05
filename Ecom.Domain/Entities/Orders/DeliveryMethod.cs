
namespace Ecom.Domain.Entities.Orders
{
    // The DeliveryMethod class represents a delivery method for an order.
    public class DeliveryMethod : BaseEntity<int>
    {
        public DeliveryMethod() { }

        public DeliveryMethod(string name, string description, decimal price, string deliveryTime)
        {
            Name = name;
            Description = description;
            Price = price;
            DeliveryTime = deliveryTime;
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string DeliveryTime { get; set; }  
    }
}
