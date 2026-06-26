using Domain.Assignments;
using Domain.Assignments.ValueObjects;
using Domain.Professionals;
using Domain.Stores;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Assignments;

public sealed class AssignmentRegistryConfiguration : IEntityTypeConfiguration<AssignmentRegistry>
{
    public void Configure(EntityTypeBuilder<AssignmentRegistry> builder)
    {
        builder.ToTable("AssignmentRegistries");

        builder.HasKey(x => x.StoreId);

        builder.Property(x => x.StoreId)
            .ValueGeneratedNever();

        builder.HasOne<Store>()
            .WithOne()
            .HasForeignKey<AssignmentRegistry>(x => x.StoreId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.OwnsMany<Assignment>("_assignments", assignments =>
        {
            assignments.ToTable("Assignments");
            assignments.WithOwner()
                .HasForeignKey("StoreId");

            assignments.Property(a => a.ProfessionalId)
                .IsRequired();
            assignments.Property(a => a.OfferingId)
                .IsRequired();

            assignments.HasOne<Professional>()
                .WithMany()
                .HasForeignKey(x => x.ProfessionalId)
                .OnDelete(DeleteBehavior.Restrict);

            assignments.HasKey(
                "StoreId",
                nameof(Assignment.ProfessionalId), nameof(Assignment.OfferingId)
            );
        });

        builder.Metadata
            .FindNavigation(nameof(AssignmentRegistry.Assignments))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}
