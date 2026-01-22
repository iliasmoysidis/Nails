using Application.Abstractions;
using Application.Exceptions;
using Application.Repositories;
using Domain.Services;

namespace Application.UseCases.Commands.StoreCatalog.RemoveOffering;

public sealed class RemoveOfferingHandler : ICommandHandler<RemoveOfferingCommand>
{
    private readonly IStoreCatalogRepository _repo;
    private readonly StoreCatalogService _service;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _uow;

    public RemoveOfferingHandler(
        IStoreCatalogRepository repo,
        StoreCatalogService service,
        ICurrentUser currentUser,
        IUnitOfWork uow
    )
    {
        _repo = repo;
        _service = service;
        _currentUser = currentUser;
        _uow = uow;
    }

    public async Task Handle(RemoveOfferingCommand command, CancellationToken ct)
    {
        var catalog = await _repo.GetAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerException("Store catalog not found.");

        var staff = await _repo.GetStaffAsync(command.StoreId, ct);

        _service.RemoveOffering(catalog, staff, _currentUser.UserId, command.OfferingId);

        await _uow.SaveChangesAsync(ct);
    }
}