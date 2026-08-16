using Application.Common.Abstractions.Authorization;
using Application.Common.Abstractions.Context;
using Application.Common.Pipelines.Command;
using Application.Professionals.Delete;
using Application.Professionals.Update;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Professionals;

public static class ProfessionalsModule
{
    public static IServiceCollection AddProfessionals(this IServiceCollection services)
    {
        services.AddScoped<UpdateProfessionalContext>();
        services.AddScoped<IAuthorizer<UpdateProfessionalCommand>, UpdateProfessionalAuthorizer>();
        services.AddScoped<IRequestContextLoader<UpdateProfessionalCommand, UpdateProfessionalContext>, UpdateProfessionalLoader>();
        services.AddTransient<IPipelineBehavior<UpdateProfessionalCommand, Unit>, ContextLoadingBehavior<UpdateProfessionalCommand, Unit, UpdateProfessionalContext>>();

        services.AddScoped<DeleteProfessionalContext>();
        services.AddScoped<IAuthorizer<DeleteProfessionalCommand>, DeleteProfessionalAuthorizer>();
        services.AddScoped<IRequestContextLoader<DeleteProfessionalCommand, DeleteProfessionalContext>, DeleteProfessionalLoader>();
        services.AddTransient<IPipelineBehavior<DeleteProfessionalCommand, Unit>, ContextLoadingBehavior<DeleteProfessionalCommand, Unit, DeleteProfessionalContext>>();

        return services;
    }
}
