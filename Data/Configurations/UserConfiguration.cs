using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nails.Models;

namespace Nails.Data.Configurations
{
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
            builder.Property(e => e.TaxIdentificationNumber).HasMaxLength(50);
            builder.HasIndex(e => e.IsProfessional);
            builder.HasIndex(e => e.IsActive);
        }
    }
}