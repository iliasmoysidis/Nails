using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nails.Models;

namespace Nails.Data.Configurations
{
    public class StoreExceptionConfiguration : IEntityTypeConfiguration<StoreException>
    {
        public void Configure(EntityTypeBuilder<StoreException> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Reason).HasMaxLength(500);

            builder.HasOne(e => e.Store)
                .WithMany(s => s.Exceptions)
                .HasForeignKey(e => e.StoreId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(e => new { e.StoreId, e.Date });
        }
    }
}