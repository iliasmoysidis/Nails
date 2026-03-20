using Application.Abstractions.Context;
using MediatR;

namespace Application.Pipelines.Query;

public sealed class QueryContextLoadingBehavior<TRequest, TResponse, TContext>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly TContext _context;
    private readonly IQueryContextLoader<TRequest, TContext> _loader;

    public QueryContextLoadingBehavior(
        TContext context,
        IQueryContextLoader<TRequest, TContext> loader
    )
    {
        _context = context;
        _loader = loader;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken ct
    )
    {
        await _loader.LoadAsync(request, _context, ct);

        return await next();
    }
}