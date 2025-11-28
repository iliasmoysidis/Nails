using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Persistence.Configurations;

public class StoreScheduleSpecialConfiguration : IEntityTypeConfiguration<StoreScheduleSpecial>
{
    public void Configure(EntityTypeBuilder<StoreScheduleSpecial> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Reason).HasMaxLength(500);

        builder.HasOne(e => e.Store)
            .WithMany(s => s.Exceptions)
            .HasForeignKey(e => e.StoreId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => new { e.StoreId, e.Date });
    }
}
