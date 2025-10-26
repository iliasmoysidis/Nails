using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nails.Models;

namespace Nails.Data.Configurations
{
    public class ProfessionalServiceConfiguration : IEntityTypeConfiguration<ProfessionalService>
    {
        public void Configure(EntityTypeBuilder<ProfessionalService> builder)
        {
            builder.HasKey(ps => new { ps.ProfessionalId, ps.ServiceId });
        }
    }
}