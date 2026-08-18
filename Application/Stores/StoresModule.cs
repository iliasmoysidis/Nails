using Application.Common.Abstractions.Authorization;
using Application.Common.Abstractions.Context;
using Application.Common.Pipelines.Command;
using Application.Stores.Close;
using Application.Stores.Create;
using Application.Stores.Update;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Stores;

public static class StoresModule
{
    public static IServiceCollection AddStores(this IServiceCollection services)
    {
        services.AddScoped<IAuthorizer<CreateStoreCommand>, CreateStoreAuthorizer>();

        services.AddScoped<CloseStoreContext>();
        services.AddScoped<IAuthorizer<CloseStoreCommand>, CloseStoreAuthorizer>();
        services.AddScoped<IRequestContextLoader<CloseStoreCommand, CloseStoreContext>, CloseStoreLoader>();
        services.AddTransient<IPipelineBehavior<CloseStoreCommand, Unit>, ContextLoadingBehavior<CloseStoreCommand, Unit, CloseStoreContext>>();

        services.AddScoped<UpdateStoreContext>();
        services.AddScoped<IAuthorizer<UpdateStoreCommand>, UpdateStoreAuthorizer>();
        services.AddScoped<IRequestContextLoader<UpdateStoreCommand, UpdateStoreContext>, UpdateStoreLoader>();
        services.AddTransient<IPipelineBehavior<UpdateStoreCommand, Unit>, ContextLoadingBehavior<UpdateStoreCommand, Unit, UpdateStoreContext>>();

        return services;
    }
}
