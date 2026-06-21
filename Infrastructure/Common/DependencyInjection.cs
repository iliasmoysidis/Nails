using Application.Appointments.Common.Repositories;
using Infrastructure.Appointments;
using Application.Common.Abstractions.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Common;

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