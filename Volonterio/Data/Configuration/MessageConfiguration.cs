using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volonterio.Data.Entities.CustomEntities;

namespace Volonterio.Data.Configuration
{
    public class MessageConfiguration : IEntityTypeConfiguration<AppMessage>
    {
        public void Configure(EntityTypeBuilder<AppMessage> builder)
        {
            builder.ToTable("tblAppMessage");

            builder.HasKey(x => x.Id);
        }
    }
}
