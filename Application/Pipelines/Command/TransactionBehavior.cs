using Application.Abstractions.UnitOfWork;
using MediatR;

namespace Application.Pipelines.Command;

public sealed class TransactionBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IUnitOfWork _uow;
    public TransactionBehavior(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken ct
    )
    {
        await _uow.BeginTransactionAsync(ct);
        try
        {
            var response = await next();

            await _uow.SaveChangesAsync(ct);

            await _uow.CommitAsync(ct);

            return response;
        }
        catch
        {
            await _uow.RollbackAsync(ct);
            throw;
        }
    }
}