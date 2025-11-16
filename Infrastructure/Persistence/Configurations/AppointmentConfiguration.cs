using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Persistence.Configurations;

public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.BookedPrice)
            .HasPrecision(10, 2)
            .IsRequired();

        builder.Property(e => e.Notes)
            .HasMaxLength(1000)
            .IsRequired(false);

        builder.Property(e => e.StartAt).IsRequired();
        builder.Property(e => e.EndAt).IsRequired();

        builder.HasOne(e => e.User)
            .WithMany()
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Professional)
            .WithMany()
            .HasForeignKey(e => e.ProfessionalId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Service)
            .WithMany()
            .HasForeignKey(e => e.ServiceId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Store)
            .WithMany()
            .HasForeignKey(e => e.StoreId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(e => new { e.ProfessionalId, e.StartAt }).HasDatabaseName("IX_Appointments_ProfessionalId_StartAt");

        builder.HasIndex(e => new { e.UserId, e.StartAt }).HasDatabaseName("IX_Appointments_UserId_StartAt");

        builder.HasIndex(e => e.Status).HasDatabaseName("IX_Appointment_Status");

        builder.HasIndex(e => e.IsActive).HasDatabaseName("IX_Appointments_IsActive");

        builder.HasIndex(e => new { e.StoreId, e.StartAt }).HasDatabaseName("IX_Appointments_StoreId_StartAt");

        builder.ToTable(t => t.HasCheckConstraint("CK_Appointment_StartBeforeEnd", "[StartAt] < [EndAt]"));
    }
}
