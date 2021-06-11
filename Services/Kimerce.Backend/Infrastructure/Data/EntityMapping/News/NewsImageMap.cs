using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kimerce.Backend.Domain.News;
using Kimerce.Backend.Domain.Images;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kimerce.Backend.Infrastructure.Data.EntityMapping.New
{
    public class NewsImageMap: IEntityMappingConfiguration<NewsImage>
    {
        public void Map(EntityTypeBuilder<NewsImage> builder)
        {
            builder.HasOne<News>(x => x.News).WithMany().HasForeignKey(x => x.NewsId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne<Image>(x => x.Image).WithMany().HasForeignKey(x => x.ImageId).OnDelete(DeleteBehavior.NoAction);
            builder.ToTable("NewsImage");
        }
    }
}
