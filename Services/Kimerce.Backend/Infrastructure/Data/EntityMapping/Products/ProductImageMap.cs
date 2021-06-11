using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kimerce.Backend.Domain.Images;
using Kimerce.Backend.Domain.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kimerce.Backend.Infrastructure.Data.EntityMapping.Products
{
    public class ProductImageMap : IEntityMappingConfiguration<ProductImage>
    {
        public void Map(EntityTypeBuilder<ProductImage> builder)
        {
            builder.HasOne<Product>(x => x.Product).WithMany().HasForeignKey(x => x.ProductId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne<Image>(x => x.Image).WithMany().HasForeignKey(x => x.ImageId).OnDelete(DeleteBehavior.NoAction);
            builder.ToTable("ProductImage");
        }
    }
}