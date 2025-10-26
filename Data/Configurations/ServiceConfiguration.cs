using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nails.Models;

namespace Nails.Data.Configurations
{
    public class ServiceConfiguration : IEntityTypeConfiguration<Service>
    {
        public void Configure(EntityTypeBuilder<Service> builder)
        {

            builder.HasKey(e => e.Id);
            builder.Property(e => e.Name).HasMaxLength(200);
            builder.Property(e => e.Price).HasPrecision(10, 2);

            builder.HasOne(e => e.Store)
                .WithMany(s => s.Services)
                .HasForeignKey(e => e.StoreId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(e => e.StoreId);
            builder.HasIndex(e => e.IsActive);
        }
    }
}