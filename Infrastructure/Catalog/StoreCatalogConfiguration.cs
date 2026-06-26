using Domain.Catalog;
using Domain.Catalog.ValueObjects;
using Domain.Common.ValueObjects;
using Domain.Stores;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Catalog;

public sealed class StoreCatalogConfiguration : IEntityTypeConfiguration<StoreCatalog>
{
    public void Configure(EntityTypeBuilder<StoreCatalog> builder)
    {
        builder.ToTable("StoreCatalogs");
        builder.HasKey(x => x.StoreId);

        builder.Property(x => x.StoreId)
            .ValueGeneratedNever();

        builder.HasOne<Store>()
            .WithOne()
            .HasForeignKey<StoreCatalog>(x => x.StoreId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.OwnsMany(x => x.Offerings, offerings =>
        {
            offerings.ToTable("Offerings");
            offerings.HasKey(x => x.Id);
            offerings.Property(x => x.Id).ValueGeneratedOnAdd();
            offerings.WithOwner().HasForeignKey("StoreId");

            offerings.OwnsOne(x => x.Name, name =>
            {
                name.Property(x => x.Value)
                    .HasColumnName("Name")
                    .HasMaxLength(200)
                    .IsRequired();
            });

            offerings.Property(x => x.Duration)
                .HasConversion(
                    v => v.Minutes,
                    v => Duration.FromMinutes(v)
                )
                .HasColumnName("DurationMinutes")
                .IsRequired();

            offerings.OwnsOne(x => x.Price, money =>
            {
                money.Property(x => x.Amount)
                    .HasColumnName("PriceAmount")
                    .HasPrecision(18, 2)
                    .IsRequired();

                money.Property(x => x.Currency)
                    .HasColumnName("PriceCurrency")
                    .HasMaxLength(3)
                    .IsRequired();
            });

            offerings.OwnsOne(x => x.Description, description =>
            {
                description.Property(x => x.Value)
                    .HasColumnName("Description")
                    .HasMaxLength(Description.MaxLength)
                    .IsRequired();
            });

            offerings.HasIndex("StoreId", "Name").IsUnique();
        });

        builder.Navigation(x => x.Offerings).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
