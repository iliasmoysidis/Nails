using Application.Appointments.Common.Repositories;
using Application.Users.Common.Queries;
using Application.Users.Common.Repositories;
using Domain.Common;
using Domain.Common.ValueObjects;
using Infrastructure.Appointments;
using Infrastructure.Users;
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
        services.AddScoped<IClock, Clock>();

        services.AddScoped<IAppointmentRepository, AppointmentRepository>();

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserQueries, UserQueries>();

        return services;
    }
}