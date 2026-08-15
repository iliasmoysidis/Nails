using Application.Common.Abstractions.Authorization;
using Application.Common.Abstractions.Context;
using Application.Users.Delete;
using Application.Users.GetDetails;
using Application.Users.Update;
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

        services.AddScoped<DeleteUserContext>();
        services.AddScoped<IAuthorizer<DeleteUserCommand>, DeleteUserAuthorizer>();
        services.AddScoped<IRequestContextLoader<DeleteUserCommand, DeleteUserContext>, DeleteUserLoader>();

        return services;
    }
}
