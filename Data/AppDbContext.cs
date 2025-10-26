using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using Microsoft.EntityFrameworkCore;
using Nails.Models;

namespace Nails.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Professional> Professionals => Set<Professional>();
        public DbSet<Store> Stores => Set<Store>();
        public DbSet<StoreManager> StoreManagers => Set<StoreManager>();
        public DbSet<StoreProfessional> StoreProfessionals => Set<StoreProfessional>();
        public DbSet<Service> Services => Set<Service>();
        public DbSet<ProfessionalService> ProfessionalServices => Set<ProfessionalService>();
        public DbSet<Appointment> Appointments => Set<Appointment>();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);


        }
    }
}