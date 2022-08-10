using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volonterio.Data.Entities.CustomEntities;

namespace Volonterio.Data.Configuration
{
    public class GroupChatConfiguration : IEntityTypeConfiguration<AppGroupChat>
    {
        public void Configure(EntityTypeBuilder<AppGroupChat> builder)
        {
            builder.ToTable("tblAppGroupChat");

            builder.HasKey(x => x.Id);
        }
    }
}
