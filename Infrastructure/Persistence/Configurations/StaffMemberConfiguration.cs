using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public sealed class StaffMemberConfiguration : IEntityTypeConfiguration<StaffMemberEntity>
{
    public void Configure(EntityTypeBuilder<StaffMemberEntity> builder)
    {
        builder.HasKey(x => new { x.StoreId, x.ProfessionalId });
    }
}