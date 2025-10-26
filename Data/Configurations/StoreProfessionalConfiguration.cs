using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nails.Models;

namespace Nails.Data.Configurations
{
    public class StoreProfessionalConfiguration : IEntityTypeConfiguration<StoreProfessional>
    {
        public void Configure(EntityTypeBuilder<StoreProfessional> builder)
        {
            builder.HasKey(sp => new { sp.StoreId, sp.ProfessionalId });
        }
    }
}