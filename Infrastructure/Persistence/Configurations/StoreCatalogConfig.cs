using Domain.Entities;
using Domain.ValueObjects.Offerings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public sealed class StoreCatalogConfig : IEntityTypeConfiguration<StoreCatalog>
{
    public void Configure(EntityTypeBuilder<StoreCatalog> builder)
    {
        builder.ToTable("store_catalogs");

        builder.HasKey(c => c.StoreId);

        builder.Property(c => c.StoreId)
            .ValueGeneratedNever();

        builder.HasMany<Offering>("_offerings")
            .WithOne()
            .HasForeignKey(o => o.StoreId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation("_offerings")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.OwnsMany<ServiceOffering>("_serviceOfferings", so =>
        {
            so.ToTable("service_offerings");

            so.WithOwner()
             .HasForeignKey("StoreId");

            so.Property<int>("StoreId");

            so.HasKey(
                "StoreId",
                nameof(ServiceOffering.ProfessionalId),
                nameof(ServiceOffering.OfferingId)
            );

            so.Property(x => x.ProfessionalId)
                .IsRequired();

            so.Property(x => x.OfferingId)
                .IsRequired();

            so.HasIndex(x => new { x.ProfessionalId, x.OfferingId });
        });

        builder.Navigation("_serviceOfferings")
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}