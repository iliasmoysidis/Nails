using Application.Common.Abstractions.Authorization;
using Application.Common.Abstractions.Context;
using Application.Common.Pipelines.Command;
using Application.Rosters.AddOwner;
using Application.Rosters.GetProfessionalStores;
using Application.Rosters.Hire;
using Application.Rosters.Terminate;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Rosters;

public static class RostersModule
{
    public static IServiceCollection AddRosters(this IServiceCollection services)
    {
        services.AddScoped<AddStoreOwnerContext>();
        services.AddScoped<IAuthorizer<AddStoreOwnerCommand>, AddStoreOwnerAuthorizer>();
        services.AddScoped<IRequestContextLoader<AddStoreOwnerCommand, AddStoreOwnerContext>, AddStoreOwnerLoader>();
        services.AddTransient<IPipelineBehavior<AddStoreOwnerCommand, Unit>, ContextLoadingBehavior<AddStoreOwnerCommand, Unit, AddStoreOwnerContext>>();

        services.AddScoped<HireStaffContext>();
        services.AddScoped<IAuthorizer<HireStaffCommand>, HireStaffAuthorizer>();
        services.AddScoped<IRequestContextLoader<HireStaffCommand, HireStaffContext>, HireStaffLoader>();
        services.AddTransient<IPipelineBehavior<HireStaffCommand, Unit>, ContextLoadingBehavior<HireStaffCommand, Unit, HireStaffContext>>();

        services.AddScoped<TerminateStaffContext>();
        services.AddScoped<IAuthorizer<TerminateStaffCommand>, TerminateStaffAuthorizer>();
        services.AddScoped<IRequestContextLoader<TerminateStaffCommand, TerminateStaffContext>, TerminateStaffLoader>();
        services.AddTransient<IPipelineBehavior<TerminateStaffCommand, Unit>, ContextLoadingBehavior<TerminateStaffCommand, Unit, TerminateStaffContext>>();

        services.AddScoped<IAuthorizer<GetProfessionalStoresQuery>, GetProfessionalStoresAuthorizer>();

        return services;
    }
}
