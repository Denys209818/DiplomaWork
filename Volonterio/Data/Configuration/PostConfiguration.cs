using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volonterio.Data.Entities.CustomEntities;

namespace Volonterio.Data.Configuration
{
    public class PostConfiguration : IEntityTypeConfiguration<AppPost>
    {
        public void Configure(EntityTypeBuilder<AppPost> builder)
        {
            builder.ToTable("tblAppPost");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(x => x.Text)
                .HasMaxLength(4000)
                .IsRequired();

            builder.HasOne(x => x.Group)
                .WithMany(x => x.Posts)
                .HasForeignKey(x => x.GroupId)
                .IsRequired();
        }
    }
}
