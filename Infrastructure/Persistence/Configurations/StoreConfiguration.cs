using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public sealed class StoreConfiguration : IEntityTypeConfiguration<Store>
{
    public void Configure(EntityTypeBuilder<Store> builder)
    {
        builder.HasKey(x => x.Id);

        builder.OwnsOne(x => x.Name, n =>
        {
            n.Property(x => x.Value)
                .IsRequired();
        });

        builder.OwnsOne(x => x.Address, a =>
        {
            a.Property(x => x.Street).IsRequired();
            a.Property(x => x.City).IsRequired();
            a.Property(x => x.PostalCode).IsRequired();
            a.Property(x => x.State).IsRequired();
            a.Property(x => x.CountryCode).IsRequired();
        });

        builder.OwnsOne(x => x.TaxIdNumber, t =>
        {
            t.Property(x => x.CountryCode).IsRequired();
            t.Property(x => x.Value).IsRequired();
        });

        builder.OwnsOne(x => x.Email, e =>
        {
            e.Property(x => x.Value).IsRequired();
        });

        builder.OwnsOne(x => x.Phone, p =>
        {
            p.Property(x => x.CountryCode).IsRequired();
            p.Property(x => x.NationalNumber).IsRequired();
        });
    }
}
