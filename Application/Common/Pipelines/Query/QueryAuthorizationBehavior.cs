using Application.Common.Abstractions.Authorization;
using MediatR;

namespace Application.Common.Pipelines.Query;

public sealed class QueryAuthorizationBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IEnumerable<IAuthorizer<TRequest>> _authorizers;

    public QueryAuthorizationBehavior(IEnumerable<IAuthorizer<TRequest>> authorizers)
    {
        _authorizers = authorizers;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken ct
    )
    {
        foreach (var authorizer in _authorizers)
        {
            await authorizer.AuthorizeAsync(request, ct);
        }

        return await next();
    }
}