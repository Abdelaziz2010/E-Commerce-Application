using Ecom.Domain.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Ecom.Infrastructure.EntitiesConfiguration
{
    public class DeliveryMethodConfiguration : IEntityTypeConfiguration<DeliveryMethod>
    {
        public void Configure(EntityTypeBuilder<DeliveryMethod> builder)
        {
            builder.Property(d => d.Price).HasColumnType("decimal(18,2)");
            builder.HasData
            (
            new DeliveryMethod { Id = 1, DeliveryTime = "Only a week", Description = "The fastest Delivery in the world", Name = "DHL", Price = 20 },
            new DeliveryMethod { Id = 2, DeliveryTime = "Only take two week", Description = "Make your product save", Name = "FedEx", Price = 15 },
            new DeliveryMethod { Id = 3, DeliveryTime = "Only take three week", Description = "Make your product save", Name = "UPS", Price = 10 }
            );
        }
    }
}
