using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Domain.Entities.Orders
{
    public class Order : BaseEntity<int>
    {
        public string BuyerEmail { get; set; }
        public decimal SubTotal { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public ShippingAddress ShippingAddress { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; } 
        public IReadOnlyList<OrderItem> OrderItems { get; set;}
        public Status Status { get; set; }
    }
}
