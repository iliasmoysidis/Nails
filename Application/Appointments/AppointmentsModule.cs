using Application.Appointments.AdjustPrice;
using Application.Appointments.Cancel;
using Application.Appointments.Complete;
using Application.Appointments.Confirm;
using Application.Appointments.Create;
using Application.Appointments.GetDetails;
using Application.Appointments.GetProfessionalAppointments;
using Application.Appointments.GetStoreAppointments;
using Application.Appointments.GetUserAppointments;
using Application.Common.Abstractions.Authorization;
using Application.Common.Abstractions.Context;
using Application.Common.Pipelines.Command;
using Application.Common.Pipelines.Query;
using Application.Appointments.MarkNoShow;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Appointments;

public static class AppointmentsModule
{
    public static IServiceCollection AddAppointments(this IServiceCollection services)
    {
        services.AddScoped<CreateAppointmentContext>();
        services.AddScoped<IAuthorizer<CreateAppointmentCommand>, CreateAppointmentAuthorizer>();
        services.AddScoped<IRequestContextLoader<CreateAppointmentCommand, CreateAppointmentContext>, CreateAppointmentLoader>();
        services.AddTransient<IPipelineBehavior<CreateAppointmentCommand, int>, ContextLoadingBehavior<CreateAppointmentCommand, int, CreateAppointmentContext>>();

        services.AddScoped<CancelAppointmentContext>();
        services.AddScoped<IAuthorizer<CancelAppointmentCommand>, CancelAppointmentAuthorizer>();
        services.AddScoped<IRequestContextLoader<CancelAppointmentCommand, CancelAppointmentContext>, CancelAppointmentLoader>();
        services.AddTransient<IPipelineBehavior<CancelAppointmentCommand, Unit>, ContextLoadingBehavior<CancelAppointmentCommand, Unit, CancelAppointmentContext>>();

        services.AddScoped<CompleteAppointmentContext>();
        services.AddScoped<IAuthorizer<CompleteAppointmentCommand>, CompleteAppointmentAuthorizer>();
        services.AddScoped<IRequestContextLoader<CompleteAppointmentCommand, CompleteAppointmentContext>, CompleteAppointmentLoader>();
        services.AddTransient<IPipelineBehavior<CompleteAppointmentCommand, Unit>, ContextLoadingBehavior<CompleteAppointmentCommand, Unit, CompleteAppointmentContext>>();

        services.AddScoped<ConfirmAppointmentContext>();
        services.AddScoped<IAuthorizer<ConfirmAppointmentCommand>, ConfirmAppointmentAuthorizer>();
        services.AddScoped<IRequestContextLoader<ConfirmAppointmentCommand, ConfirmAppointmentContext>, ConfirmAppointmentLoader>();
        services.AddTransient<IPipelineBehavior<ConfirmAppointmentCommand, Unit>, ContextLoadingBehavior<ConfirmAppointmentCommand, Unit, ConfirmAppointmentContext>>();

        services.AddScoped<AdjustAppointmentPriceContext>();
        services.AddScoped<IAuthorizer<AdjustAppointmentPriceCommand>, AdjustAppointmentPriceAuthorizer>();
        services.AddScoped<IRequestContextLoader<AdjustAppointmentPriceCommand, AdjustAppointmentPriceContext>, AdjustAppointmentPriceLoader>();
        services.AddTransient<IPipelineBehavior<AdjustAppointmentPriceCommand, Unit>, ContextLoadingBehavior<AdjustAppointmentPriceCommand, Unit, AdjustAppointmentPriceContext>>();

        services.AddScoped<MarkAppointmentNoShowContext>();
        services.AddScoped<IAuthorizer<MarkAppointmentNoShowCommand>, MarkAppointmentNoShowAuthorizer>();
        services.AddScoped<IRequestContextLoader<MarkAppointmentNoShowCommand, MarkAppointmentNoShowContext>, MarkAppointmentNoShowLoader>();
        services.AddTransient<IPipelineBehavior<MarkAppointmentNoShowCommand, Unit>, ContextLoadingBehavior<MarkAppointmentNoShowCommand, Unit, MarkAppointmentNoShowContext>>();

        services.AddScoped<GetAppointmentDetailsContext>();
        services.AddScoped<IAuthorizer<GetAppointmentDetailsQuery>, GetAppointmentDetailsAuthorizer>();
        services.AddScoped<IQueryContextLoader<GetAppointmentDetailsQuery, GetAppointmentDetailsContext>, GetAppointmentDetailsLoader>();
        services.AddTransient<IPipelineBehavior<GetAppointmentDetailsQuery, AppointmentDetailsDTO>, QueryContextLoadingBehavior<GetAppointmentDetailsQuery, AppointmentDetailsDTO, GetAppointmentDetailsContext>>();

        services.AddScoped<IAuthorizer<GetProfessionalAppointmentsQuery>, GetProfessionalAppointmentsAuthorizer>();
        services.AddScoped<IAuthorizer<GetStoreAppointmentsQuery>, GetStoreAppointmentsAuthorizer>();
        services.AddScoped<IAuthorizer<GetUserAppointmentsQuery>, GetUserAppointmentsAuthorizer>();

        return services;
    }
}
