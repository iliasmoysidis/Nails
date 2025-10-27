using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nails.Models;

namespace Nails.Data.Configurations
{
    public class StoreConfiguration : IEntityTypeConfiguration<Store>
    {
        public void Configure(EntityTypeBuilder<Store> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasIndex(e => e.Email).IsUnique();
            builder.Property(e => e.Name).HasMaxLength(200);
            builder.Property(e => e.Address).HasMaxLength(500);
            builder.Property(e => e.TaxIdentificationNumber).HasMaxLength(50);
            builder.Property(e => e.Email).HasMaxLength(255);
            builder.Property(e => e.Phone).HasMaxLength(20);
            builder.HasIndex(e => e.IsActive);
        }
    }
}