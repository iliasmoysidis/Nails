using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => e.Email).IsUnique();
        builder.Property(e => e.Email).HasMaxLength(255);
        builder.Property(e => e.Name).HasMaxLength(100);
        builder.Property(e => e.Surname).HasMaxLength(100);
        builder.Property(e => e.Phone).HasMaxLength(20);
        builder.HasIndex(e => e.IsActive);
    }
}
