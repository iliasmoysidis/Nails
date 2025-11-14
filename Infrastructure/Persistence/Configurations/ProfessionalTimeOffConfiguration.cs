using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Persistence.Configurations;

public class ProfessionalTimeOffConfiguration : IEntityTypeConfiguration<ProfessionalTimeOff>
{
    public void Configure(EntityTypeBuilder<ProfessionalTimeOff> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Reason).HasMaxLength(500);

        builder.HasOne(e => e.Professional)
            .WithMany(u => u.TimeOffs)
            .HasForeignKey(e => e.ProfessionalId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Store)
            .WithMany()
            .HasForeignKey(e => e.StoreId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(e => new { e.ProfessionalId, e.StartAt, e.EndAt });
    }
}
