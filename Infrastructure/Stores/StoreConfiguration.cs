using Domain.Stores;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Stores;

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
            a.Property(x => x.Street)
                .HasColumnName("Street")
                .IsRequired();

            a.Property(x => x.City)
                .HasColumnName("City")
                .IsRequired();

            a.Property(x => x.PostalCode)
                .HasColumnName("PostalCode")
                .IsRequired();

            a.Property(x => x.State)
                .HasColumnName("State")
                .IsRequired();

            a.Property(x => x.CountryCode)
                .HasColumnName("CountryCode")
                .IsRequired();
        });

        builder.OwnsOne(x => x.TaxIdNumber, t =>
        {
            t.Property(x => x.CountryCode)
                .HasColumnName("TaxCountryCode")
                .IsRequired();

            t.Property(x => x.Value)
                .HasColumnName("TaxIdNumber")
                .IsRequired();
        });

        builder.OwnsOne(x => x.Email, e =>
        {
            e.Property(x => x.Value)
                .HasColumnName("Email")
                .IsRequired();
        });

        builder.OwnsOne(x => x.Phone, p =>
        {
            p.Property(x => x.CountryCode)
                .HasColumnName("PhoneCountryCode")
                .IsRequired();

            p.Property(x => x.NationalNumber)
                .HasColumnName("PhoneNationalNumber")
                .IsRequired();
        });

        builder.HasIndex("Email").IsUnique();

        builder.HasIndex("PhoneCountryCode", "PhoneNationalNumber").IsUnique();
    }
}
