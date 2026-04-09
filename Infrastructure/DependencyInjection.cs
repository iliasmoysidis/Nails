using Application.Abstractions.Repositories;
using Application.Abstractions.UnitOfWork;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<AppDbContext>(options => options.UseSqlServer(config.GetConnectionString("Default")));

        services.AddScoped<IUnitOfWork, EfUnitOfWork>();

        services.AddScoped<IAppointmentRepository, AppointmentRepository>();

        return services;
    }
}