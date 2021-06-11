using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kimerce.Backend.Domain.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kimerce.Backend.Infrastructure.Data.EntityMapping.Products
{
    public class ProductPieceMap : IEntityMappingConfiguration<ProductPiece>
    {
        public void Map(EntityTypeBuilder<ProductPiece> builder)
        {
            builder.ToTable("ProductPiece");
        }
    }
}
