using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public sealed class StaffConfiguration : IEntityTypeConfiguration<StaffEntity>
{
    public void Configure(EntityTypeBuilder<StaffEntity> builder)
    {
        builder.HasKey(x => x.StoreId);

        builder.HasMany(x => x.Members)
        .WithOne()
        .HasForeignKey(x => x.StoreId)
        .OnDelete(DeleteBehavior.Cascade);
    }
}