using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volonterio.Data.Entities.CustomEntities;

namespace Volonterio.Data.Configuration
{
    public class PostTagEntitiesConfiguration : IEntityTypeConfiguration<AppPostTagEntity>
    {
        public void Configure(EntityTypeBuilder<AppPostTagEntity> builder)
        {
            builder.ToTable("tblAppPostTagEntities");
            builder.HasKey( x=> new {x.PostId, x.PostTagId});

            builder.HasOne(x => x.PostTag)
                .WithMany(x => x.PostTagEntities)
                .HasForeignKey(x => x.PostTagId)
                .IsRequired();

            builder.HasOne(x => x.Post)
                .WithMany(x => x.PostTagEntities)
                .HasForeignKey(x => x.PostId)
                .IsRequired();
        }
    }
}
