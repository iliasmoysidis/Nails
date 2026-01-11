using Domain.Entities;
using Domain.ValueObjects.Time;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Persistence.Configurations;

public sealed class AppointmentConfig : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.ToTable("appointments");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(a => a.StartAt)
            .HasConversion(
                v => v.Value,
                v => UtcDateTime.From(v)
            )
            .IsRequired();

        builder.OwnsOne(a => a.Duration, duration =>
        {
            duration.Property(x => x)
                 .HasColumnName("duration_minutes")
                 .HasConversion(
                     v => (int)v.Value.TotalMinutes,
                     v => Duration.FromMinutes(v)
                 )
                 .IsRequired();
        });

        builder.OwnsOne(a => a.BookedPrice, money =>
        {
            money.Property(m => m.Amount)
                .HasColumnType("decimal(10,2)")
                .IsRequired();

            money.Property(m => m.Currency)
                .HasMaxLength(3)
                .IsRequired();
        });

        builder.OwnsOne(a => a.Notes, n =>
        {
            n.Property(x => x.Value)
                 .HasColumnName("notes")
                 .HasMaxLength(500);
        });

        builder.Property(a => a.CanceledAt)
            .HasConversion(
            new ValueConverter<UtcDateTime?, DateTime?>(
                v => v.HasValue ? v.Value.Value : null,
                v => v.HasValue ? UtcDateTime.From(v.Value) : null
            )
        );

        builder.HasIndex(a => new { a.ProfessionalId, a.StartAt });
        builder.HasIndex(a => a.StoreId);
        builder.HasIndex(a => a.UserId);
    }
}