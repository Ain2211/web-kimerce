using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kimerce.Backend.Domain.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kimerce.Backend.Infrastructure.Data.EntityMapping.Products
{
    public class ProductCategoryMap : IEntityMappingConfiguration<ProductCategory>
    {
        public void Map(EntityTypeBuilder<ProductCategory> builder)
        {
            builder.HasMany(p => p.Children).WithOne(p => p.Parent).HasForeignKey(p => p.ParentId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}