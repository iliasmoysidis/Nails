using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<StoreCatalog> StoreCatalogs => Set<StoreCatalog>();
    public DbSet<StoreCalendar> StoreCalendars => Set<StoreCalendar>();
    public DbSet<StaffCalendar> StaffCalendars => Set<StaffCalendar>();
    public DbSet<Staff> Staffs => Set<Staff>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
