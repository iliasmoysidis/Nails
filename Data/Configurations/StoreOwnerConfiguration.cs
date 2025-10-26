using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nails.Models;

namespace Nails.Data.Configurations
{
    public class StoreManagerConfiguration : IEntityTypeConfiguration<StoreManager>
    {
        public void Configure(EntityTypeBuilder<StoreManager> builder)
        {
            builder.HasKey(so => new { so.UserId, so.StoreId });
        }
    }
}