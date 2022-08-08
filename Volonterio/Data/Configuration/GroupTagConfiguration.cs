using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volonterio.Data.Entities.CustomEntities;

namespace Volonterio.Data.Configuration
{
    public class GroupTagConfiguration : IEntityTypeConfiguration<AppGroupTag>
    {
        public void Configure(EntityTypeBuilder<AppGroupTag> builder)
        {
            builder.ToTable("tblAppGroupTag");
            builder.HasKey(x => new { x.GroupId, x.TagId});

            builder.HasOne(x => x.Group)
                .WithMany(x => x.AppGroupTags)
                .HasForeignKey(x => x.GroupId)
                .IsRequired();

            builder.HasOne(x => x.Tag)
                .WithMany(x => x.AppGroupTags)
                .HasForeignKey(x => x.TagId)
                .IsRequired();
        }
    }
}
