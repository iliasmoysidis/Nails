using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public sealed class AssignmentConfiguration : IEntityTypeConfiguration<AssignmentEntity>
{
    public void Configure(EntityTypeBuilder<AssignmentEntity> builder)
    {
        builder.ToTable("Assignments");

        builder.HasKey(x => new
        {
            x.StoreId,
            x.ProfessionalId,
            x.OfferingId
        });

        builder.Property(x => x.StoreId).IsRequired();
        builder.Property(x => x.ProfessionalId).IsRequired();
        builder.Property(x => x.OfferingId).IsRequired();
    }
}