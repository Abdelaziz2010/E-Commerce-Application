using Ecom.Domain.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Ecom.Infrastructure.EntitiesConfiguration
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {

        public void Configure(EntityTypeBuilder<Order> builder)
        {
            // relationship between Order and ShippingAddress
            builder.OwnsOne(o => o.ShippingAddress, n => { n.WithOwner(); });                // 1:1 relationship

            // relationship between Order and OrderItem 
            builder.HasMany(o => o.OrderItems).WithOne().OnDelete(DeleteBehavior.Cascade);   // 1:n relationship

            // relationship between Order and DeliveryMethod 
            builder.HasOne(o => o.DeliveryMethod).WithMany().OnDelete(DeleteBehavior.Restrict);  // n:1 relationship

            // set Status as a string in the database
            builder.Property(o => o.Status).HasConversion(s => s.ToString(), s => (Status)Enum.Parse(typeof(Status), s));

            builder.Property(o => o.SubTotal).HasColumnType("decimal(18,2)");
        }
    }
}
