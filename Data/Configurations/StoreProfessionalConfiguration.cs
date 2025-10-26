using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nails.Models;

namespace Nails.Data.Configurations
{
    public class StoreProfessionalConfiguration : IEntityTypeConfiguration<StoreProfessional>
    {
        public void Configure(EntityTypeBuilder<StoreProfessional> builder)
        {
            builder.HasKey(e => new { e.StoreId, e.ProfessionalId });

            builder.HasOne(e => e.Store)
                .WithMany(s => s.Staff)
                .HasForeignKey(e => e.StoreId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Professional)
                .WithMany(u => u.Workplaces)
                .HasForeignKey(e => e.ProfessionalId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(e => new { e.StoreId, e.EndDate });
        }
    }
}