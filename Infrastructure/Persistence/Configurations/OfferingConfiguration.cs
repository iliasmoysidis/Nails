using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public sealed class OfferingConfiguration : IEntityTypeConfiguration<Offering>
{
    public void Configure(EntityTypeBuilder<Offering> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.StoreId).IsRequired();

        builder.OwnsOne(x => x.Name, name =>
        {
            name.Property(n => n.Value)
                .HasColumnName("Name")
                .IsRequired();
        });

        builder.OwnsOne(x => x.Price, money =>
        {
            money.Property(m => m.Amount)
                .IsRequired();
            money.Property(m => m.Currency)
                .IsRequired();
        });

        builder.OwnsOne(x => x.Duration, d =>
        {
            d.Property(x => x.Value)
                .HasColumnName("DurationMinutes")
                .IsRequired();
        });

        builder.OwnsOne(x => x.Description, d =>
        {
            d.Property(x => x.Value)
                .IsRequired(false);
        });
    }
}
