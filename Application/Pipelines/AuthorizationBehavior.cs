using Application.Abstractions.Authorization;
using MediatR;

namespace Application.Pipelines;

public sealed class AuthorizationBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IEnumerable<IAuthorizer<TRequest>> _authorizers;

    public AuthorizationBehavior(IEnumerable<IAuthorizer<TRequest>> authorizers)
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