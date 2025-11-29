using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Persistence.Configurations;

public class ProfessionalServiceConfiguration : IEntityTypeConfiguration<StaffService>
{
    public void Configure(EntityTypeBuilder<StaffService> builder)
    {
        builder.HasKey(e => new { e.ProfessionalId, e.ServiceId });

        builder.HasOne(e => e.Professional)
            .WithMany(p => p.ProvidedServices)
            .HasForeignKey(e => e.ProfessionalId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Service)
            .WithMany(p => p.Providers)
            .HasForeignKey(e => e.ServiceId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(e => e.ServiceId)
            .HasDatabaseName("IX_ProfessionalServices_ServiceId");
    }
}
