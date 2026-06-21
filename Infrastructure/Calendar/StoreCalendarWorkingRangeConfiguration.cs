using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Calendar;

public sealed class StoreCalendarWorkingRangeConfiguration : IEntityTypeConfiguration<StoreCalendarWorkingRangeEntity>
{
    public void Configure(EntityTypeBuilder<StoreCalendarWorkingRangeEntity> builder)
    {
        builder.ToTable("StoreCalendarWorkingRanges", t =>
        {
            t.HasCheckConstraint("CK_StoreCalendarWorkingRange_TimeRange",
            "Start < End");
        });

        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.StoreId)
            .IsRequired();

        builder
            .Property(x => x.Day)
            .HasConversion<int>()
            .IsRequired();

        builder
            .Property(x => x.Start)
            .IsRequired();

        builder
            .Property(x => x.End)
            .IsRequired();

        builder.HasIndex(x => new { x.StoreId, x.Day });
    }
}