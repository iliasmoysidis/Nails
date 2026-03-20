using Application.Abstractions.Context;
using MediatR;

public sealed class ContextLoadingBehavior<TRequest, TResponse, TContext>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
    where TContext : notnull
{
    private readonly IRequestContextLoader<TRequest, TContext> _loader;
    private readonly TContext _ctx;

    public ContextLoadingBehavior(
        IRequestContextLoader<TRequest, TContext> loader,
        TContext ctx)
    {
        _loader = loader;
        _ctx = ctx;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken ct)
    {
        await _loader.PopulateAsync(request, _ctx, ct);

        return await next();
    }
}