using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Kimerce.Backend.Domain.Setting;

namespace Kimerce.Backend.Infrastructure.Data.EntityMapping.Locations
{
    public class SettingMap : IEntityMappingConfiguration<Setting>
    {
        public void Map(EntityTypeBuilder<Setting> builder)
        {
        }
    }
}
