using Kimerce.Backend.Domain.Images;
using Kimerce.Backend.Domain.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kimerce.Backend.Infrastructure.Data.EntityMapping.Products
{
    public class ProductMap : IEntityMappingConfiguration<Product>
    {
        public void Map(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Product");
        }
    }
}