using Kimerce.Backend.Domain.Categories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kimerce.Backend.Infrastructure.Data.EntityMapping.Locations
{

    public class BaseCategoryMap : IEntityMappingConfiguration<BaseCategory>
    {
        public void Map(EntityTypeBuilder<BaseCategory> builder)
        {
            builder.ToTable("Category");
        }
    }
}
