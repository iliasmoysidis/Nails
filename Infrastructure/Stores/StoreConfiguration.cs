
using Domain.Common.ValueObjects;
using Domain.Stores;
using Domain.Stores.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Stores;

public sealed class StoreConfiguration : IEntityTypeConfiguration<Store>
{
    public void Configure(EntityTypeBuilder<Store> builder)
    {
        builder.ToTable("Stores");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder.OwnsOne(x => x.Name, n =>
        {
            n.Property(x => x.Value)
                .HasColumnName("StoreName")
                .HasMaxLength(StoreName.MaxLength)
                .IsRequired();
        });

        builder.OwnsOne(x => x.Address, address =>
        {

            address.Property(x => x.Street)
                .HasColumnName("Street")
                .HasMaxLength(Address.StreetMaxLength)
                .IsRequired();

            address.Property(x => x.City)
                .HasColumnName("City")
                .HasMaxLength(Address.CityMaxLength)
                .IsRequired();

            address.Property(x => x.PostalCode)
                .HasColumnName("PostalCode")
                .HasMaxLength(Address.PostalCodeMaxLength)
                .IsRequired();

            address.Property(x => x.State)
                .HasColumnName("State")
                .HasMaxLength(Address.StateMaxLength)
                .IsRequired();

            address.Property(x => x.CountryCode)
                .HasColumnName("CountryCode")
                .HasMaxLength(Address.CountryCodeMaxLength)
                .IsRequired();
        });

        builder.OwnsOne(x => x.TaxIdNumber, taxIdNumber =>
        {
            taxIdNumber.Property(x => x.CountryCode)
                .HasColumnName("TaxCountryCode")
                .HasMaxLength(TaxIdentificationNumber.CountryCodeMaxLength)
                .IsRequired();

            taxIdNumber.Property(x => x.Value)
                .HasColumnName("TaxIdNumber")
                .HasMaxLength(TaxIdentificationNumber.TaxIdNumberMaxLength)
                .IsRequired();
        });

        builder.OwnsOne(x => x.Email, email =>
        {
            email.Property(x => x.Value)
                .HasColumnName("Email")
                .HasMaxLength(Email.MaxLength)
                .IsRequired();
        });

        builder.OwnsOne(x => x.Phone, phone =>
        {
            phone.Property(x => x.CountryCode)
                .HasColumnName("PhoneCountryCode")
                .HasMaxLength(Phone.CountryCodeMaxLength)
                .IsRequired();

            phone.Property(x => x.NationalNumber)
                .HasColumnName("PhoneNationalNumber")
                .HasMaxLength(Phone.NationalNumberMaxLength)
                .IsRequired();
        });

        builder.Property(x => x.IsClosed)
            .IsRequired();

        builder.OwnsOne(x => x.ClosedAt, closedAt =>
        {
            closedAt.Property(x => x.Value)
                .HasColumnName("ClosedAt")
                .HasColumnType("datetime2");
        });

        builder.Navigation(x => x.Name)
            .IsRequired();

        builder.Navigation(x => x.Address)
            .IsRequired();

        builder.Navigation(x => x.TaxIdNumber)
            .IsRequired();

        builder.Navigation(x => x.Email)
            .IsRequired();

        builder.Navigation(x => x.Phone)
            .IsRequired();

        builder.HasIndex("Email").IsUnique();

        builder.HasIndex("TaxCountryCode", "TaxIdNumber")
            .IsUnique();

        builder.HasIndex("PhoneCountryCode", "PhoneNationalNumber")
            .IsUnique();
    }
}
