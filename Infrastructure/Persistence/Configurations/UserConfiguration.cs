using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);

        builder.OwnsOne(x => x.FullName, f =>
        {
            f.Property(x => x.FirstName)
                .HasColumnName("FirstName")
                .IsRequired();

            f.Property(x => x.LastName)
                .HasColumnName("LastName")
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
