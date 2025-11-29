using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Store> Stores => Set<Store>();
    public DbSet<StoreManager> StoreManagers => Set<StoreManager>();
    public DbSet<StoreStaff> StoreStaffs => Set<StoreStaff>();
    public DbSet<Service> Services => Set<Service>();
    public DbSet<StaffService> StaffServices => Set<StaffService>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<StoreSchedule> StoreSchedule => Set<StoreSchedule>();
    public DbSet<StoreScheduleSpecial> StoreScheduleSpecials => Set<StoreScheduleSpecial>();
    public DbSet<ProfessionalSchedule> ProfessionalSchedules => Set<ProfessionalSchedule>();
    public DbSet<ProfessionalTimeOff> ProfessionalTimeOffs => Set<ProfessionalTimeOff>();


    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>().HasQueryFilter(e => e.IsActive);
        modelBuilder.Entity<Store>().HasQueryFilter(e => e.IsActive);
        modelBuilder.Entity<Service>().HasQueryFilter(e => e.IsActive);
        modelBuilder.Entity<Appointment>().HasQueryFilter(e => e.IsActive);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
