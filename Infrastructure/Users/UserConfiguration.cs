using Domain.Common.ValueObjects;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Users;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

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

        builder.Property(x => x.IsDeleted)
            .IsRequired();

        builder.OwnsOne(x => x.DeletedAt, deletedAt =>
        {
            deletedAt.Property(x => x.Value)
                .HasColumnName("DeletedAt")
                .HasColumnType("datetime2");
        });

        builder.Navigation(x => x.FullName)
            .IsRequired();

        builder.Navigation(x => x.Email)
            .IsRequired();

        builder.Navigation(x => x.Phone)
            .IsRequired();

        builder.HasIndex("Email").IsUnique();

        builder.HasIndex("PhoneCountryCode", "PhoneNationalNumber")
            .IsUnique();
    }
}
