using Ecom.Domain.Entities.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Infrastructure.EntitiesConfiguration
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Description).IsRequired();
            builder.Property(x => x.NewPrice).HasColumnType("decimal(18,2)");
            builder.Property(x => x.OldPrice).HasColumnType("decimal(18,2)");
            builder.HasData(
                new Product { Id = 1, Name = "test", Description = "test", NewPrice = 10.00m, CategoryId = 1 },
                new Product { Id = 2, Name = "test2", Description = "test2", NewPrice = 20.00m, CategoryId = 2 },
                new Product { Id = 3, Name = "test3", Description = "test3", NewPrice = 30.00m, CategoryId = 3 }
            );
        }
    }
}
