using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public sealed class StoreCalendarExceptionConfiguration : IEntityTypeConfiguration<StoreCalendarExceptionEntity>
{
    public void Configure(EntityTypeBuilder<StoreCalendarExceptionEntity> builder)
    {
        builder.ToTable("StoreCalendarExceptions", t =>
        {
            t.HasCheckConstraint(
                "CK_StoreCalendarException_TimeRange",
                @"
                (IsDayOff = 1 AND Start IS NULL AND End IS NULL)
                OR
                (IsDayOff = 0 AND Start IS NOT NULL AND End IS NOT NULL AND Start < End)
                "
            );
        });

        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.StoreId)
            .IsRequired();

        builder
            .Property(x => x.Date)
            .IsRequired();

        builder
            .Property(x => x.Start)
            .IsRequired(false);

        builder
            .Property(x => x.End)
            .IsRequired(false);

        builder.HasIndex(x => new { x.StoreId, x.Date });
    }
}