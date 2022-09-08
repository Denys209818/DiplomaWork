using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volonterio.Data.Entities.CustomEntities;

namespace Volonterio.Data.Configuration
{
    public class GroupMessageConfiguration : IEntityTypeConfiguration<AppGroupMessage>
    {
        public void Configure(EntityTypeBuilder<AppGroupMessage> builder)
        {
            builder.ToTable("tblAppGroupMessage");
            builder.HasKey(x => new {x.Id});

            builder.HasOne(x => x.User)
                .WithMany(x => x.GroupMessages)
                .HasForeignKey(x => x.UserId)
                .IsRequired();

            builder.HasOne(x => x.Group)
                .WithMany(x => x.GroupMessages)
                .HasForeignKey(x => x.GroupId)
                .IsRequired();

            builder.Property(x => x.Message)
                .IsRequired()
                .HasMaxLength(255)
                ;

        }
    }
}
