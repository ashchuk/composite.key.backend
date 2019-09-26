using CompositeKey.Common;
using CompositeKey.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CompositeKey.Infrastructure.Map
{
    public class AreaMap : DbEntityConfiguration<Area>
    {
        public override void Configure(EntityTypeBuilder<Area> entity)
        {
            entity.ToTable("Areas");

            entity.HasKey(k => k.Id);
            entity.Property(e => e.Id).IsRequired().ValueGeneratedOnAdd();

            entity.Property(p => p.Name).IsRequired().HasMaxLength(200);
            entity.Property(p => p.CreatedOn).IsRequired();
            entity.Property(p => p.CreatedById).IsRequired();
            entity.Property(p => p.UpdatedOn).IsRequired(false);
            entity.Property(p => p.UpdatedById).IsRequired(false);

            entity.HasOne(e => e.CreatedBy)
                .WithMany(e => e.CreatedAreas)
                .HasForeignKey(e => e.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.UpdatedBy)
                .WithMany(e => e.UpdatedAreas)
                .HasForeignKey(e => e.UpdatedById)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
