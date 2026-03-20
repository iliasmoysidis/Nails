using Application.Abstractions.Validation;
using MediatR;

namespace Application.Pipelines.Command;

public sealed class ValidationBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IEnumerable<IRequestValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IRequestValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken ct
    )
    {
        foreach (var validator in _validators)
        {
            await validator.ValidateAsync(request, ct);
        }

        return await next();
    }
}