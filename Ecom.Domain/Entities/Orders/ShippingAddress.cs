
namespace Ecom.Domain.Entities.Orders
{
    public class ShippingAddress : BaseEntity<int>
    {
        public ShippingAddress() { }
        public ShippingAddress(string firstName, string lastName, string state, string city, string street, string zipCode)
        {
            FirstName = firstName;
            LastName = lastName;
            State = state;
            City = city;
            Street = street;
            ZipCode = zipCode;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string ZipCode { get; set; }
    }
}