using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volonterio.Data.Entities.CustomEntities;

namespace Volonterio.Data.Configuration
{
    public class PostImageConfiguration : IEntityTypeConfiguration<AppPostImage>
    {
        public void Configure(EntityTypeBuilder<AppPostImage> builder)
        {
            builder.ToTable("tblAppPostImage");

            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Post)
                .WithMany(x => x.Images)
                .HasForeignKey(x => x.PostId)
                .IsRequired();

            builder.Property(x => x.Image)
                .HasMaxLength(255)
                .IsRequired();
        }
    }
}
