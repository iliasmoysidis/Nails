namespace Application.Common.Abstractions.Context;

public interface IRequestContextLoader<TRequest, TContext>
{
    Task PopulateAsync(TRequest request, TContext context, CancellationToken ct);
}