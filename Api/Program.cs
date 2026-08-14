using Api.Common.Auth;
using Api.Common.Errors;
using Application.Common.Abstractions.Authorization;
using Application.Common.Abstractions.Context;
using Application.Common.Abstractions.Events;
using Application.Common.Contexts;
using Application.Common.Pipelines.Command;
using Application.Common.Pipelines.Query;
using Application.Common.Services;
using Application.Users.Delete;
using Application.Users.GetProfile;
using Application.Users.Update;
using Infrastructure.Common;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddControllers();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IRequestContext, HttpRequestContext>();

builder.Services.AddExceptionHandler<ApplicationExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehavior<,>));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ContextLoadingBehavior<,,>));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(QueryAuthorizationBehavior<,>));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(QueryContextLoadingBehavior<,,>));

builder.Services.AddScoped<IDomainEventDispatcher, MediatRDomainEventDispatcher>();

builder.Services.AddMediatR(cfg =>
    {
        cfg.RegisterServicesFromAssembly(typeof(Application.AssemblyMarker).Assembly);
    }
);

builder.Services.AddScoped<IAuthorizer<GetUserProfileQuery>, GetUserProfileAuthorizer>();

builder.Services.AddScoped<UpdateUserContext>();
builder.Services.AddScoped<IAuthorizer<UpdateUserCommand>, UpdateUserAuthorizer>();
builder.Services.AddScoped<IRequestContextLoader<UpdateUserCommand, UpdateUserContext>, UpdateUserLoader>();

builder.Services.AddScoped<DeleteUserContext>();
builder.Services.AddScoped<IAuthorizer<DeleteUserCommand>, DeleteUserAuthorizer>();
builder.Services.AddScoped<IRequestContextLoader<DeleteUserCommand, DeleteUserContext>, DeleteUserLoader>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseExceptionHandler();
app.UseHttpsRedirection();

app.MapControllers();

app.Run();
