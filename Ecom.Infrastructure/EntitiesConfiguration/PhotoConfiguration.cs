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
    public class PhotoConfiguration : IEntityTypeConfiguration<Photo>
    {
        public void Configure(EntityTypeBuilder<Photo> builder)
        {
            builder.HasData(
                new Photo { Id = 1, ImageName = "test1", ProductId = 1 },
                new Photo { Id = 2, ImageName = "test2", ProductId = 2 },
                new Photo { Id = 3, ImageName = "test3", ProductId = 3 });
        }
    }
}
