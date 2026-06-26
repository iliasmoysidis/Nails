using Domain.Professionals;
using Domain.Roster;
using Domain.Roster.ValueObjects;
using Domain.Stores;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Roster;

public sealed class StaffConfiguration : IEntityTypeConfiguration<Staff>
{
    public void Configure(EntityTypeBuilder<Staff> builder)
    {
        builder.ToTable("Staff");

        builder.HasKey(x => x.StoreId);

        builder.Property(x => x.StoreId)
            .ValueGeneratedNever();

        builder.HasOne<Store>()
            .WithOne()
            .HasForeignKey<Staff>(x => x.StoreId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.OwnsMany(x => x.Members, members =>
        {
            members.ToTable("StaffMembers");

            members.WithOwner()
                .HasForeignKey("StoreId");

            members.HasKey(x => x.ProfessionalId);

            members.Property(x => x.ProfessionalId)
                .ValueGeneratedNever();

            members.HasOne<Professional>()
                .WithMany()
                .HasForeignKey(x => x.ProfessionalId)
                .OnDelete(DeleteBehavior.Restrict);

            members.OwnsMany(x => x.Roles, roles =>
            {
                roles.ToTable("StaffMemberRoles");

                roles.WithOwner()
                    .HasForeignKey("StoreId", "ProfessionalId");

                roles.Property(x => x.Role)
                    .HasConversion<int>()
                    .IsRequired();

                roles.HasKey("StoreId", "ProfessionalId", nameof(RoleAssignment.Role));
            });

            members.Navigation(x => x.Roles)
                .UsePropertyAccessMode(PropertyAccessMode.Field);
        });

        builder.Navigation(x => x.Members)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
