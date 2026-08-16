using Application.Common.Abstractions.Authorization;
using Application.Common.Abstractions.Context;
using Application.Common.Pipelines.Command;
using Application.Users.Delete;
using Application.Users.GetDetails;
using Application.Users.Update;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Users;

public static class UsersModule
{
    public static IServiceCollection AddUsers(this IServiceCollection services)
    {
        services.AddScoped<IAuthorizer<GetUserDetailsQuery>, GetUserDetailsAuthorizer>();

        services.AddScoped<UpdateUserContext>();
        services.AddScoped<IAuthorizer<UpdateUserCommand>, UpdateUserAuthorizer>();
        services.AddScoped<IRequestContextLoader<UpdateUserCommand, UpdateUserContext>, UpdateUserLoader>();
        services.AddTransient<IPipelineBehavior<UpdateUserCommand, Unit>, ContextLoadingBehavior<UpdateUserCommand, Unit, UpdateUserContext>>();

        services.AddScoped<DeleteUserContext>();
        services.AddScoped<IAuthorizer<DeleteUserCommand>, DeleteUserAuthorizer>();
        services.AddScoped<IRequestContextLoader<DeleteUserCommand, DeleteUserContext>, DeleteUserLoader>();
        services.AddTransient<IPipelineBehavior<DeleteUserCommand, Unit>, ContextLoadingBehavior<DeleteUserCommand, Unit, DeleteUserContext>>();

        return services;
    }
}
