using Application.Common.Abstractions.Events;
using Application.Common.Pipelines.Command;
using Application.Common.Pipelines.Query;
using Application.Common.Services;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));

builder.Services.AddScoped<IDomainEventDispatcher, MediatRDomainEventDispatcher>();

builder.Services.AddMediatR(cfg =>
    {
        cfg.RegisterServicesFromAssembly(typeof(CloseStoreHandler).Assembly);
    }
);

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(QueryAuthorizationBehavior<,>));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();