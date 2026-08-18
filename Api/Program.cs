using Api.Common.Auth;
using Api.Common.Errors;
using Application.Appointments;
using Application.Assignments;
using Application.Common.Abstractions.Events;
using Application.Common.Contexts;
using Application.Common.Guards;
using Application.Common.Pipelines.Command;
using Application.Common.Pipelines.Query;
using Application.Common.Services;
using Application.Professionals;
using Application.Users;
using Infrastructure.Common;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddControllers();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IRequestContext, HttpRequestContext>();
builder.Services.AddScoped<AuthorizationGuard>();

builder.Services.AddExceptionHandler<ApplicationExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehavior<,>));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(QueryAuthorizationBehavior<,>));

builder.Services.AddScoped<IDomainEventDispatcher, MediatRDomainEventDispatcher>();

builder.Services.AddMediatR(cfg =>
    {
        cfg.RegisterServicesFromAssembly(typeof(Application.AssemblyMarker).Assembly);
    }
);

builder.Services.AddUsers();
builder.Services.AddProfessionals();
builder.Services.AddAssignments();
builder.Services.AddAppointments();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseExceptionHandler();
app.UseHttpsRedirection();

app.MapControllers();

app.Run();
