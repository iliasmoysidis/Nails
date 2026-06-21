namespace Application.Common.Abstractions.Context;

public interface IQueryContextLoader<TRequest, TContext>
{
    Task LoadAsync(TRequest request, TContext context, CancellationToken ct);
}