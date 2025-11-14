using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Persistence.Configurations;

public class StoreManagerConfiguration : IEntityTypeConfiguration<StoreManager>
{
    public void Configure(EntityTypeBuilder<StoreManager> builder)
    {
        builder.HasKey(e => new { e.StoreId, e.UserId });

        builder.HasOne(e => e.Store)
            .WithMany(s => s.Managers)
            .HasForeignKey(e => e.StoreId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.User)
            .WithMany(u => u.ManagedStores)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => new { e.StoreId, e.Role, e.EndDate });
    }
}
