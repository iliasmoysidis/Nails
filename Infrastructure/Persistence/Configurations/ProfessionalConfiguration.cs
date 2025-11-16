using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Persistence.Configurations;

public class ProfessionalConfiguration : IEntityTypeConfiguration<Professional>
{
    public void Configure(EntityTypeBuilder<Professional> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(e => e.Surname)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(e => e.Email)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(e => e.Phone)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(e => e.TaxIdNumber)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(e => e.Email)
            .IsUnique()
            .HasDatabaseName("IX_Professionals_Email");

        builder.HasIndex(e => e.TaxIdNumber)
            .IsUnique()
            .HasDatabaseName("IX_Professionals_TaxIdNumber");

        builder.HasIndex(e => e.IsActive)
            .HasDatabaseName("IX_Professionals_IsActive");

        builder.Metadata
            .FindNavigation(nameof(Professional.Workplaces))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.Metadata
            .FindNavigation(nameof(Professional.ProvidedServices))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.Metadata
            .FindNavigation(nameof(Professional.TimeOffs))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.Metadata
            .FindNavigation(nameof(Professional.ManagedStores))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}
