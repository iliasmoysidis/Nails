using Domain.Appointments;
using Domain.Appointments.ValueObjects;
using Domain.Common.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Appointments;

public sealed class AppointmentConfiguration
    : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.ToTable("Appointments");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.UserId).IsRequired();
        builder.Property(x => x.ProfessionalId).IsRequired();
        builder.Property(x => x.OfferingId).IsRequired();
        builder.Property(x => x.StoreId).IsRequired();


        builder.Property(x => x.StartAt)
            .HasConversion(
                v => v.Value,
                v => UtcDateTime.FromUtc(
                    DateTime.SpecifyKind(v, DateTimeKind.Utc)))
            .HasColumnType("datetime2")
            .IsRequired();

        builder.OwnsOne(x => x.Duration, duration =>
        {
            duration.Property(x => x.Value)
                .HasColumnName("DurationMinutes")
                .IsRequired();
        });

        builder.Property(x => x.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.CanceledAt)
            .HasConversion(
                v => v.HasValue
                    ? v.Value.Value
                    : (DateTime?)null,
                v => v.HasValue
                    ? UtcDateTime.FromUtc(
                        DateTime.SpecifyKind(
                            v.Value,
                            DateTimeKind.Utc))
                    : null)
            .HasColumnType("datetime2");

        builder.OwnsOne(x => x.Price, money =>
        {
            money.Property(x => x.Amount)
                .HasColumnName("PriceAmount")
                .HasPrecision(18, 2)
                .IsRequired();

            money.Property(x => x.Currency)
                .HasColumnName("PriceCurrency")
                .HasMaxLength(3)
                .IsRequired();
        });

        builder.OwnsOne(x => x.Notes, notes =>
        {
            notes.Property(x => x.Value)
                .HasColumnName("Notes")
                .HasMaxLength(Notes.MaxLength)
                .IsRequired();
        });

        builder.Navigation(x => x.Duration)
            .IsRequired();

        builder.Navigation(x => x.Price)
            .IsRequired();

        builder.Navigation(x => x.Notes)
            .IsRequired();

        builder.HasIndex(x => x.UserId);

        builder.HasIndex(x => x.ProfessionalId);

        builder.HasIndex(x => x.StoreId);

        builder.HasIndex(x => x.StartAt);

        builder.HasIndex(x => new
        {
            x.ProfessionalId,
            x.StartAt
        });
    }
}
