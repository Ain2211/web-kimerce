using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kimerce.Backend.Domain.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kimerce.Backend.Infrastructure.Data.EntityMapping.Products
{
    public class RelateProductMap : IEntityMappingConfiguration<RelateProduct>
    {
        public void Map(EntityTypeBuilder<RelateProduct> builder)
        {
            builder.HasOne(r => r.ProductA).WithMany(p => p.RelateChildren).HasForeignKey(r => r.ProductIdA).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(r => r.ProductB).WithMany(p => p.RelateParent).HasForeignKey(r => r.ProductIdB).OnDelete(DeleteBehavior.NoAction);
            builder.ToTable("RelateProduct");
        }
    }
}