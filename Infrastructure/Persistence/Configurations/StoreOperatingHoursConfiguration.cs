using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Persistence.Configurations;

public class StoreOperatingHoursConfiguration : IEntityTypeConfiguration<StoreOperatingHours>
{
    public void Configure(EntityTypeBuilder<StoreOperatingHours> builder)
    {
        builder.HasKey(e => e.Id);

        builder.HasOne(e => e.Store)
            .WithMany(s => s.OperatingHours)
            .HasForeignKey(e => e.StoreId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => new { e.StoreId, e.DayOfWeek }).IsUnique();
    }
}
