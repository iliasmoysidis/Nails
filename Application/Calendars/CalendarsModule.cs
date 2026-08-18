using Application.Calendars.AddHoliday;
using Application.Calendars.AddSpecialHours;
using Application.Calendars.RemoveException;
using Application.Calendars.SetDayOff;
using Application.Calendars.SetWorkingDay;
using Application.Common.Abstractions.Authorization;
using Application.Common.Abstractions.Context;
using Application.Common.Pipelines.Command;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Calendars;

public static class CalendarsModule
{
    public static IServiceCollection AddCalendars(this IServiceCollection services)
    {
        services.AddScoped<AddHolidayContext>();
        services.AddScoped<IAuthorizer<AddHolidayCommand>, AddHolidayAuthorizer>();
        services.AddScoped<IRequestContextLoader<AddHolidayCommand, AddHolidayContext>, AddHolidayLoader>();
        services.AddTransient<IPipelineBehavior<AddHolidayCommand, Unit>, ContextLoadingBehavior<AddHolidayCommand, Unit, AddHolidayContext>>();

        services.AddScoped<AddSpecialHoursContext>();
        services.AddScoped<IAuthorizer<AddSpecialHoursCommand>, AddSpecialHoursAuthorizer>();
        services.AddScoped<IRequestContextLoader<AddSpecialHoursCommand, AddSpecialHoursContext>, AddSpecialHoursLoader>();
        services.AddTransient<IPipelineBehavior<AddSpecialHoursCommand, Unit>, ContextLoadingBehavior<AddSpecialHoursCommand, Unit, AddSpecialHoursContext>>();

        services.AddScoped<RemoveCalendarExceptionContext>();
        services.AddScoped<IAuthorizer<RemoveCalendarExceptionCommand>, RemoveCalendarExceptionAuthorizer>();
        services.AddScoped<IRequestContextLoader<RemoveCalendarExceptionCommand, RemoveCalendarExceptionContext>, RemoveCalendarExceptionLoader>();
        services.AddTransient<IPipelineBehavior<RemoveCalendarExceptionCommand, Unit>, ContextLoadingBehavior<RemoveCalendarExceptionCommand, Unit, RemoveCalendarExceptionContext>>();

        services.AddScoped<SetCalendarDayOffContext>();
        services.AddScoped<IAuthorizer<SetCalendarDayOffCommand>, SetCalendarDayOffAuthorizer>();
        services.AddScoped<IRequestContextLoader<SetCalendarDayOffCommand, SetCalendarDayOffContext>, SetCalendarDayOffLoader>();
        services.AddTransient<IPipelineBehavior<SetCalendarDayOffCommand, Unit>, ContextLoadingBehavior<SetCalendarDayOffCommand, Unit, SetCalendarDayOffContext>>();

        services.AddScoped<SetCalendarWorkingDayContext>();
        services.AddScoped<IAuthorizer<SetCalendarWorkingDayCommand>, SetCalendarWorkingDayAuthorizer>();
        services.AddScoped<IRequestContextLoader<SetCalendarWorkingDayCommand, SetCalendarWorkingDayContext>, SetCalendarWorkingDayLoader>();
        services.AddTransient<IPipelineBehavior<SetCalendarWorkingDayCommand, Unit>, ContextLoadingBehavior<SetCalendarWorkingDayCommand, Unit, SetCalendarWorkingDayContext>>();

        return services;
    }
}
