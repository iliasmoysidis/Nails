using Application.Appointments.Common.Queries;
using Application.Appointments.Common.Repositories;
using Application.Assignments.Common.Queries;
using Application.Assignments.Common.Repositories;
using Application.Calendars.Common.Queries;
using Application.Calendars.Common.Repositories;
using Application.Catalogs.Common.Queries;
using Application.Catalogs.Common.Repositories;
using Application.Professionals.Common.Queries;
using Application.Professionals.Common.Repositories;
using Application.Rosters.Common.Queries;
using Application.Rosters.Common.Repositories;
using Application.Schedules.Common.Queries;
using Application.Schedules.Common.Repositories;
using Application.Stores.Common.Queries;
using Application.Stores.Common.Repositories;
using Application.Users.Common.Queries;
using Application.Users.Common.Repositories;
using Domain.Common;
using Domain.Common.ValueObjects;
using Infrastructure.Appointments;
using Infrastructure.Assignments;
using Infrastructure.Calendars;
using Infrastructure.Catalogs;
using Infrastructure.Professionals;
using Infrastructure.Rosters;
using Infrastructure.Schedules;
using Infrastructure.Stores;
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
        services.AddScoped<IAppointmentQueries, AppointmentQueries>();

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserQueries, UserQueries>();

        services.AddScoped<IAssignmentRegistryRepository, AssignmentRegistryRepository>();
        services.AddScoped<IAssignmentRegistryQueries, AssignmentRegistryQueries>();

        services.AddScoped<IStoreCalendarRepository, StoreCalendarRepository>();
        services.AddScoped<IStoreCalendarQueries, StoreCalendarQueries>();

        services.AddScoped<IStoreCatalogRepository, StoreCatalogRepository>();
        services.AddScoped<IStoreCatalogQueries, StoreCatalogQueries>();

        services.AddScoped<IProfessionalRepository, ProfessionalRepository>();
        services.AddScoped<IProfessionalQueries, ProfessionalQueries>();

        services.AddScoped<IStaffRepository, StaffRepository>();
        services.AddScoped<IStaffQueries, StaffQueries>();

        services.AddScoped<IProfessionalScheduleRepository, ProfessionalScheduleRepository>();
        services.AddScoped<IProfessionalScheduleQueries, ProfessionalScheduleQueries>();

        services.AddScoped<IStoreRepository, StoreRepository>();
        services.AddScoped<IStoreQueries, StoreQueries>();

        return services;
    }
}
