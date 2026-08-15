using Application.Common.Abstractions.Authorization;
using Application.Common.Abstractions.Context;
using Application.Professionals.Delete;
using Application.Professionals.Update;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Professionals;

public static class ProfessionalsModule
{
    public static IServiceCollection AddProfessionals(this IServiceCollection services)
    {
        services.AddScoped<UpdateProfessionalContext>();
        services.AddScoped<IAuthorizer<UpdateProfessionalCommand>, UpdateProfessionalAuthorizer>();
        services.AddScoped<IRequestContextLoader<UpdateProfessionalCommand, UpdateProfessionalContext>, UpdateProfessionalLoader>();

        services.AddScoped<DeleteProfessionalContext>();
        services.AddScoped<IAuthorizer<DeleteProfessionalCommand>, DeleteProfessionalAuthorizer>();
        services.AddScoped<IRequestContextLoader<DeleteProfessionalCommand, DeleteProfessionalContext>, DeleteProfessionalLoader>();

        return services;
    }
}
