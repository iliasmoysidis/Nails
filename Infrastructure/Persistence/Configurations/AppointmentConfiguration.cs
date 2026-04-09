using Domain.Entities;
using Domain.ValueObjects.Time;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence;

public sealed class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.UserId).IsRequired();
        builder.Property(x => x.StartAt).HasConversion(v => v.Value, v => UtcDateTime.FromUtc(v));

        builder.OwnsOne(x => x.Price, money =>
        {
            money.Property(m => m.Amount);
            money.Property(m => m.Currency);
        });
    }
}