using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nails.Models;

namespace Nails.Data.Configurations
{
    public class ProfessionalServiceConfiguration : IEntityTypeConfiguration<ProfessionalService>
    {
        public void Configure(EntityTypeBuilder<ProfessionalService> builder)
        {
            builder.HasKey(e => new { e.ProfessionalId, e.ServiceId });

            builder.HasOne(e => e.Professional)
                .WithMany(u => u.ProvidedServices)
                .HasForeignKey(e => e.ProfessionalId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Service)
                .WithMany(s => s.Providers)
                .HasForeignKey(e => e.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}