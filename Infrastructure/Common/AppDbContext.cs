using Domain.Roster;
using Domain.Appointments;
using Domain.Professionals;
using Domain.Stores;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Domain.Assignments;
using Domain.Schedule;
using Domain.Calendar;
using Domain.Catalog;

namespace Infrastructure.Common;

public sealed class AppDbContext : DbContext
{
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<Store> Stores => Set<Store>();
    public DbSet<Professional> Professionals => Set<Professional>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Staff> Staff => Set<Staff>();
    public DbSet<AssignmentRegistry> Assignments => Set<AssignmentRegistry>();
    public DbSet<ProfessionalSchedule> ProfessionalSchedules => Set<ProfessionalSchedule>();
    public DbSet<StoreCalendar> StoreCalendars => Set<StoreCalendar>();
    public DbSet<StoreCatalog> StoreCatalogs => Set<StoreCatalog>();


    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
