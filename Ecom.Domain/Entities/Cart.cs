
namespace Ecom.Domain.Entities
{
    // Cart class represents a shopping cart in the e-commerce system
    public class Cart
    {
        public Cart() { }
        public Cart(string id)
        {
            Id = id;
        }

        public string Id { get; set; }   // Key
        public List<CartItem> CartItems { get; set; } = new List<CartItem>();  // Value
        //public string PaymentIntentId { get; set; }
        //public string ClientSecret { get; set; }
    }
}
