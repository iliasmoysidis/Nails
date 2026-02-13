using Application.Abstractions.Policies.Stores;
using Application.Abstractions.Services;
using Application.Abstractions.UnitOfWork;
using Domain.Interfaces;

namespace Application.Commands.Stores;

public sealed class DeleteStoreHandler
{
    private readonly IManageStorePolicy _policy;
    private readonly IStoreClosureService _service;
    private readonly IUnitOfWork _uow;

    public DeleteStoreHandler(
        IManageStorePolicy policy,
        IStoreClosureService service,
        IUnitOfWork uow
    )
    {
        _policy = policy;
        _service = service;
        _uow = uow;
    }

    public async Task Handle(DeleteStoreCommand command, CancellationToken ct)
    {
        await _policy.EnsureCanManageAsync(command.StoreId, ct);

        await _service.CloseAsync(command.StoreId, ct);

        await _uow.SaveChangesAsync(ct);
    }
}