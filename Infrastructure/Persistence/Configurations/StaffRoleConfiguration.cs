using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public sealed class StaffRoleConfiguration : IEntityTypeConfiguration<StaffRoleEntity>
{
    public void Configure(EntityTypeBuilder<StaffRoleEntity> builder)
    {
        builder.HasKey(x => new { x.StoreId, x.ProfessionalId, x.Role });

        builder.Property(x => x.Role).HasConversion<int>();
    }
}