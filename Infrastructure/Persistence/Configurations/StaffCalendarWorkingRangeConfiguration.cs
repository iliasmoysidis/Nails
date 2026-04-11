using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public sealed class StaffCalendarWorkingRangeConfiguration : IEntityTypeConfiguration<StaffCalendarWorkingRangeEntity>
{
    public void Configure(EntityTypeBuilder<StaffCalendarWorkingRangeEntity> builder)
    {
        builder.ToTable("StaffWorkingRanges", t =>
        {
            t.HasCheckConstraint(
                "CK_StaffWorkingRange_TimeRange",
                "Start < End"
            );
        });

        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.StoreId)
            .IsRequired();

        builder
            .Property(x => x.ProfessionalId)
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
    }
}