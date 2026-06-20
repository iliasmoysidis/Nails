using Domain.Professionals;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public sealed class ProfessionalConfiguration : IEntityTypeConfiguration<Professional>
{
    public void Configure(EntityTypeBuilder<Professional> builder)
    {
        builder.HasKey(x => x.Id);

        builder.OwnsOne(x => x.FullName, f =>
        {
            f.Property(x => x.FirstName);
            f.Property(x => x.LastName);
        });

        builder.OwnsOne(x => x.Email, e =>
        {
            e.Property(x => x.Value);
        });

        builder.OwnsOne(x => x.TaxIdNumber, t =>
        {
            t.Property(x => x.CountryCode);
            t.Property(x => x.Value);
        });
    }
}