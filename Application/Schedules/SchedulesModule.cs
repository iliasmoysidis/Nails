using Application.Common.Abstractions.Authorization;
using Application.Common.Abstractions.Context;
using Application.Common.Pipelines.Command;
using Application.Schedules.AddSpecialAvailability;
using Application.Schedules.AddVacation;
using Application.Schedules.GetExceptions;
using Application.Schedules.GetWeeklySchedule;
using Application.Schedules.RemoveException;
using Application.Schedules.SetDayOff;
using Application.Schedules.SetWorkingDay;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Schedules;

public static class SchedulesModule
{
    public static IServiceCollection AddSchedules(this IServiceCollection services)
    {
        services.AddScoped<AddSpecialAvailabilityContext>();
        services.AddScoped<IAuthorizer<AddSpecialAvailabilityCommand>, AddSpecialAvailabilityAuthorizer>();
        services.AddScoped<IRequestContextLoader<AddSpecialAvailabilityCommand, AddSpecialAvailabilityContext>, AddSpecialAvailabilityLoader>();
        services.AddTransient<IPipelineBehavior<AddSpecialAvailabilityCommand, Unit>, ContextLoadingBehavior<AddSpecialAvailabilityCommand, Unit, AddSpecialAvailabilityContext>>();

        services.AddScoped<AddVacationContext>();
        services.AddScoped<IAuthorizer<AddVacationCommand>, AddVacationAuthorizer>();
        services.AddScoped<IRequestContextLoader<AddVacationCommand, AddVacationContext>, AddVacationLoader>();
        services.AddTransient<IPipelineBehavior<AddVacationCommand, Unit>, ContextLoadingBehavior<AddVacationCommand, Unit, AddVacationContext>>();

        services.AddScoped<RemoveScheduleExceptionContext>();
        services.AddScoped<IAuthorizer<RemoveScheduleExceptionCommand>, RemoveScheduleExceptionAuthorizer>();
        services.AddScoped<IRequestContextLoader<RemoveScheduleExceptionCommand, RemoveScheduleExceptionContext>, RemoveScheduleExceptionLoader>();
        services.AddTransient<IPipelineBehavior<RemoveScheduleExceptionCommand, Unit>, ContextLoadingBehavior<RemoveScheduleExceptionCommand, Unit, RemoveScheduleExceptionContext>>();

        services.AddScoped<SetScheduleDayOffContext>();
        services.AddScoped<IAuthorizer<SetScheduleDayOffCommand>, SetScheduleDayOffAuthorizer>();
        services.AddScoped<IRequestContextLoader<SetScheduleDayOffCommand, SetScheduleDayOffContext>, SetScheduleDayOffLoader>();
        services.AddTransient<IPipelineBehavior<SetScheduleDayOffCommand, Unit>, ContextLoadingBehavior<SetScheduleDayOffCommand, Unit, SetScheduleDayOffContext>>();

        services.AddScoped<SetScheduleWorkingDayContext>();
        services.AddScoped<IAuthorizer<SetScheduleWorkingDayCommand>, SetScheduleWorkingDayAuthorizer>();
        services.AddScoped<IRequestContextLoader<SetScheduleWorkingDayCommand, SetScheduleWorkingDayContext>, SetScheduleWorkingDayLoader>();
        services.AddTransient<IPipelineBehavior<SetScheduleWorkingDayCommand, Unit>, ContextLoadingBehavior<SetScheduleWorkingDayCommand, Unit, SetScheduleWorkingDayContext>>();

        services.AddScoped<IAuthorizer<GetScheduleExceptionsQuery>, GetScheduleExceptionsAuthorizer>();
        services.AddScoped<IAuthorizer<GetWeeklyScheduleQuery>, GetWeeklyScheduleAuthorizer>();

        return services;
    }
}
