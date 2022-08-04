using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volonterio.Data.Entities.CustomEntities;

namespace Volonterio.Data.Configuration
{
    public class GroupConfiguration : IEntityTypeConfiguration<AppGroup>
    {
        public void Configure(EntityTypeBuilder<AppGroup> builder)
        {
            builder.ToTable("tblAppGroup");

            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.User)
                .WithMany(x => x.Groups)
                .HasForeignKey(x => x.UserId)
                .IsRequired();

            builder.Property(x => x.Title)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property<string>(x => x.Description)
                .HasMaxLength(4000)
                .IsRequired();

            builder.Property(x => x.Meta)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(x => x.Image)
                .HasMaxLength(255);
        }
    }
}
