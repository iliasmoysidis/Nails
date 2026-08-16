using Application.Assignments.Add;
using Application.Assignments.Remove;
using Application.Common.Abstractions.Authorization;
using Application.Common.Abstractions.Context;
using Application.Common.Pipelines.Command;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Assignments;

public static class AssignmentsModule
{
    public static IServiceCollection AddAssignments(this IServiceCollection services)
    {
        services.AddScoped<IAuthorizer<AddAssignmentsCommand>, AddAssignmentsAuthorizer>();
        services.AddScoped<AddAssignmentsContext>();
        services.AddScoped<IRequestContextLoader<AddAssignmentsCommand, AddAssignmentsContext>, AddAssignmentsLoader>();
        services.AddTransient<IPipelineBehavior<AddAssignmentsCommand, Unit>, ContextLoadingBehavior<AddAssignmentsCommand, Unit, AddAssignmentsContext>>();

        services.AddScoped<IAuthorizer<RemoveAssignmentsCommand>, RemoveAssignmentsAuthorizer>();
        services.AddScoped<RemoveAssignmentsContext>();
        services.AddScoped<IRequestContextLoader<RemoveAssignmentsCommand, RemoveAssignmentsContext>, RemoveAssignmentsLoader>();
        services.AddTransient<IPipelineBehavior<RemoveAssignmentsCommand, Unit>, ContextLoadingBehavior<RemoveAssignmentsCommand, Unit, RemoveAssignmentsContext>>();

        return services;
    }
}
