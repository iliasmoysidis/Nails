using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Persistence.Configurations;

public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.BookedPrice).HasPrecision(10, 2);
        builder.Property(e => e.Notes).HasMaxLength(1000);

        builder.HasIndex(e => new { e.ProfessionalId, e.StartAt });
        builder.HasIndex(e => new { e.UserId, e.StartAt });
        builder.HasIndex(e => e.Status);
        builder.HasIndex(e => e.IsActive);
    }
}
