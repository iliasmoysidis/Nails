using Domain.Entities;
using Domain.Enums;
using Domain.ValueObjects.Staff;
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

        builder.OwnsMany<StaffMember>("_members", m =>
        {
            m.ToTable("staff_members");

            m.WithOwner()
             .HasForeignKey("StoreId");

            m.Property<int>("StoreId");

            m.Property(x => x.ProfessionalId)
             .IsRequired();

            m.HasKey("StoreId", nameof(StaffMember.ProfessionalId));

            m.OwnsMany<StaffMemberRole>("_roles", r =>
            {
                r.ToTable("staff_member_roles");

                r.WithOwner()
                    .HasForeignKey("StoreId", "ProfessionalId");

                r.Property<int>("StoreId");
                r.Property<int>("ProfessionalId");

                r.Property(x => x.Role)
                    .HasColumnName("RoleId")
                    .HasConversion<int>()
                    .IsRequired();

                r.HasKey("StoreId", "ProfessionalId", "RoleId");
            });

            m.Navigation("_roles")
                .UsePropertyAccessMode(PropertyAccessMode.Field);
        });

        builder.Navigation("_members")
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
