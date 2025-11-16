using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Persistence.Configurations;

public class ProfessionalScheduleConfiguration : IEntityTypeConfiguration<ProfessionalSchedule>
{
    public void Configure(EntityTypeBuilder<ProfessionalSchedule> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.DayOfWeek)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(e => e.StartTime)
            .IsRequired();

        builder.Property(e => e.EndTime)
            .IsRequired();

        builder.Property(e => e.IsActive)
            .IsRequired();

        builder.HasOne(e => e.Professional)
            .WithMany(p => p.WorkSchedules)
            .HasForeignKey(e => e.ProfessionalId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Store)
            .WithMany()
            .HasForeignKey(e => e.StoreId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => new { e.ProfessionalId, e.StoreId, e.DayOfWeek, e.IsActive }).HasDatabaseName("IX_ProfessionalSchedules_Composite");

        builder.HasIndex(e => new { e.ProfessionalId, e.StoreId, e.DayOfWeek, e.StartTime })
        .IsUnique()
        .HasFilter("[IsActive] = 1")
        .HasDatabaseName("IX_ProfessionalSchedules_Unique_Active");

        builder.ToTable(t => t.HasCheckConstraint(
            "CK_ProfessionalSchedule_StartBeforeEnd",
            "[StartTime]<[EndTime]"));
    }
}
