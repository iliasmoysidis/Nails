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

        builder.HasOne(e => e.Customer)
            .WithMany(u => u.BookedAppointments)
            .HasForeignKey(e => e.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Professional)
            .WithMany(u => u.ProvidedAppointments)
            .HasForeignKey(e => e.ProfessionalId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Service)
            .WithMany(s => s.Appointments)
            .HasForeignKey(e => e.ServiceId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Store)
            .WithMany(s => s.Appointments)
            .HasForeignKey(e => e.StoreId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(e => new { e.ProfessionalId, e.StartAt });
        builder.HasIndex(e => new { e.CustomerId, e.StartAt });
        builder.HasIndex(e => e.Status);
        builder.HasIndex(e => e.IsActive);
    }
}
