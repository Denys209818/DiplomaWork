using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volonterio.Data.Entities.CustomEntities;

namespace Volonterio.Data.Configuration
{
    public class TagConfiguration : IEntityTypeConfiguration<AppTag>
    {
        public void Configure(EntityTypeBuilder<AppTag> builder)
        {
            builder.HasKey(x => x.Id);

            builder.ToTable("tblAppTag");

            builder.HasOne(x => x.Group)
                .WithMany(x => x.Tags)
                .HasForeignKey(x => x.GroupId)
                .IsRequired();

            builder.Property(x => x.Tag)
                .HasMaxLength(255)
                .IsRequired();
        }
    }
}
