using Domain.Common.ValueObjects;
using Domain.Professionals;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Professionals;

public sealed class ProfessionalConfiguration : IEntityTypeConfiguration<Professional>
{
    public void Configure(EntityTypeBuilder<Professional> builder)
    {
        builder.ToTable("Professionals");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder.OwnsOne(x => x.FullName, fullName =>
        {
            fullName.Property(x => x.FirstName)
                .HasColumnName("FirstName")
                .HasMaxLength(FullName.MaxLength)
                .IsRequired();

            fullName.Property(x => x.LastName)
                .HasColumnName("LastName")
                .HasMaxLength(FullName.MaxLength)
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

        builder.Property(x => x.IsDeleted)
            .IsRequired();

        builder.OwnsOne(x => x.DeletedAt, deletedAt =>
        {
            deletedAt.Property(x => x.Value)
                .HasColumnName("DeletedAt")
                .HasColumnType("datetime2")
                .IsRequired();
        });

        builder.Navigation(x => x.FullName)
            .IsRequired();

        builder.Navigation(x => x.Email)
            .IsRequired();

        builder.Navigation(x => x.Phone)
            .IsRequired();

        builder.Navigation(x => x.TaxIdNumber)
            .IsRequired();

        builder.HasIndex("Email")
            .IsUnique();

        builder.HasIndex("PhoneCountryCode", "PhoneNationalNumber")
            .IsUnique();

        builder.HasIndex("TaxCountryCode", "TaxIdNumber")
            .IsUnique();
    }
}
