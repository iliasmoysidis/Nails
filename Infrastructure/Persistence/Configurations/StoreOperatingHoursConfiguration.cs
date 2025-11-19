using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Persistence.Configurations;

public class StoreScheduleConfiguration : IEntityTypeConfiguration<StoreSchedule>
{
    public void Configure(EntityTypeBuilder<StoreSchedule> builder)
    {
        builder.HasKey(e => e.Id);

        builder.HasOne(e => e.Store)
            .WithMany(s => s.storeSchedule)
            .HasForeignKey(e => e.StoreId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => new { e.StoreId, e.DayOfWeek }).IsUnique();
    }
}
