using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nails.Models;

namespace Nails.Data.Configurations
{
    public class StoreOwnerConfiguration : IEntityTypeConfiguration<StoreOwner>
    {
        public void Configure(EntityTypeBuilder<StoreOwner> builder)
        {
            builder.HasKey(so => new { so.UserId, so.StoreId });
        }
    }
}