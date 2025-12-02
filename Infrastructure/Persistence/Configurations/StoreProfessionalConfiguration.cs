using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Persistence.Configurations;

public class StoreStaffConfiguration : IEntityTypeConfiguration<StoreEmployee>
{
    public void Configure(EntityTypeBuilder<StoreEmployee> builder)
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
