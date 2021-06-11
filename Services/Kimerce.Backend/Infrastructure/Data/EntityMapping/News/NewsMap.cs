using Kimerce.Backend.Domain.Images;
using Kimerce.Backend.Domain.News;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kimerce.Backend.Infrastructure.Data.EntityMapping.New
{
    public class NewsMap : IEntityMappingConfiguration<News>
    {
        public void Map(EntityTypeBuilder<News> builder)
        {
            builder.ToTable("News");
        }

        
    }
}
