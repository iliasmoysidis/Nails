using Domain.Entities;
using Domain.ValueObjects.Store;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public sealed class StaffConfig : IEntityTypeConfiguration<Staff>
{
    public void Configure(EntityTypeBuilder<Staff> builder)
    {
        builder.ToTable("staff");

        builder.HasKey(s => s.StoreId);

        builder.Property(s => s.StoreId)
            .ValueGeneratedNever();

        builder.OwnsMany<Owner>("_owners", o =>
        {
            o.ToTable("store_owners");

            o.WithOwner()
                .HasForeignKey("StoreId");

            o.Property<int>("StoreId");

            o.Property(x => x.ProfessionalId)
                .IsRequired();

            o.HasKey("StoreId", nameof(Owner.ProfessionalId));
        });

        builder.Navigation("_owners")
           .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.OwnsMany<Employee>("_employees", e =>
        {
            e.ToTable("store_employees");

            e.WithOwner()
                .HasForeignKey("StoreId");

            e.Property<int>("StoreId");

            e.Property(x => x.ProfessionalId)
                .IsRequired();

            e.HasKey("StoreId", nameof(Employee.ProfessionalId));
        });

        builder.Navigation("_employees")
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}