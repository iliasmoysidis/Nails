using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public sealed class AppDbContext : DbContext
{
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<Store> Stores => Set<Store>();
    public DbSet<Staff> staff => Set<Staff>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}