using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Roster;

public sealed class StaffMemberConfiguration : IEntityTypeConfiguration<StaffMemberEntity>
{
    public void Configure(EntityTypeBuilder<StaffMemberEntity> builder)
    {
        builder.HasKey(x => new { x.StoreId, x.ProfessionalId });
    }
}