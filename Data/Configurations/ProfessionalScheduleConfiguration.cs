using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nails.Models;

namespace Nails.Data.Configurations
{
    public class ProfessionalScheduleConfiguration : IEntityTypeConfiguration<ProfessionalSchedule>
    {
        public void Configure(EntityTypeBuilder<ProfessionalSchedule> builder)
        {
            builder.HasKey(e => e.Id);

            builder.HasOne(e => e.Professional)
                .WithMany(u => u.WorkSchedules)
                .HasForeignKey(e => e.ProfessionalId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Store)
                .WithMany()
                .HasForeignKey(e => e.StoreId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(e => new { e.ProfessionalId, e.StoreId, e.DayOfWeek, e.IsActive });
        }
    }
}