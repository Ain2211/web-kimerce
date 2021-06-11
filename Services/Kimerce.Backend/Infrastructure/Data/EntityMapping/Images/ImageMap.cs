using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Kimerce.Backend.Domain.Images;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kimerce.Backend.Infrastructure.Data.EntityMapping
{
    public class ImageMap : IEntityMappingConfiguration<Image>
    {
        public void Map(EntityTypeBuilder<Image> builder)
        {
            builder.ToTable("Image");
        }
    }
}
