
namespace Ecom.Domain.Entities
{
    // CartItem class represents an item in a shopping cart
    public class CartItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
    }
}
