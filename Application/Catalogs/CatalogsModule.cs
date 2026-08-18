using Application.Catalogs.Create;
using Application.Catalogs.Remove;
using Application.Catalogs.Update;
using Application.Common.Abstractions.Authorization;
using Application.Common.Abstractions.Context;
using Application.Common.Pipelines.Command;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Catalogs;

public static class CatalogsModule
{
    public static IServiceCollection AddCatalogs(this IServiceCollection services)
    {
        services.AddScoped<CreateOfferingContext>();
        services.AddScoped<IAuthorizer<CreateOfferingCommand>, CreateOfferingAuthorizer>();
        services.AddScoped<IRequestContextLoader<CreateOfferingCommand, CreateOfferingContext>, CreateOfferingLoader>();
        services.AddTransient<IPipelineBehavior<CreateOfferingCommand, int>, ContextLoadingBehavior<CreateOfferingCommand, int, CreateOfferingContext>>();

        services.AddScoped<UpdateOfferingContext>();
        services.AddScoped<IAuthorizer<UpdateOfferingCommand>, UpdateOfferingAuthorizer>();
        services.AddScoped<IRequestContextLoader<UpdateOfferingCommand, UpdateOfferingContext>, UpdateOfferingLoader>();
        services.AddTransient<IPipelineBehavior<UpdateOfferingCommand, Unit>, ContextLoadingBehavior<UpdateOfferingCommand, Unit, UpdateOfferingContext>>();

        services.AddScoped<RemoveOfferingContext>();
        services.AddScoped<IAuthorizer<RemoveOfferingCommand>, RemoveOfferingAuthorizer>();
        services.AddScoped<IRequestContextLoader<RemoveOfferingCommand, RemoveOfferingContext>, RemoveOfferingLoader>();
        services.AddTransient<IPipelineBehavior<RemoveOfferingCommand, Unit>, ContextLoadingBehavior<RemoveOfferingCommand, Unit, RemoveOfferingContext>>();

        return services;
    }
}
