using Domain.Entities;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public sealed class AppDbContext : DbContext
{
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<Store> Stores => Set<Store>();
    public DbSet<Offering> Offerings => Set<Offering>();
    public DbSet<Professional> Professionals => Set<Professional>();
    public DbSet<User> Users => Set<User>();


    public DbSet<AssignmentEntity> Assignments => Set<AssignmentEntity>();
    public DbSet<StaffEntity> Staff => Set<StaffEntity>();
    public DbSet<StaffMemberEntity> StaffMembers => Set<StaffMemberEntity>();
    public DbSet<StaffRoleEntity> StaffRoles => Set<StaffRoleEntity>();
    public DbSet<StaffCalendarWorkingRangeEntity> StaffCalendarWorkingRanges => Set<StaffCalendarWorkingRangeEntity>();
    public DbSet<StaffCalendarExceptionEntity> StaffCalendarExceptions => Set<StaffCalendarExceptionEntity>();
    public DbSet<StoreCalendarWorkingRangeEntity> StoreCalendarWorkingRanges => Set<StoreCalendarWorkingRangeEntity>();
    public DbSet<StoreCalendarExceptionEntity> StoreCalendarExceptions => Set<StoreCalendarExceptionEntity>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
