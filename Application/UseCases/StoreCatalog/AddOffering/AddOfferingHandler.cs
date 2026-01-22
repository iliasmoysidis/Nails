using Application.Abstractions;
using Application.Exceptions;
using Application.Repositories;
using Domain.Services;

namespace Application.UseCases.StoreCatalog.AddOffering;

public sealed class AddOfferingHandler : ICommandHandler<AddOfferingCommand>
{
    private readonly IStoreCatalogRepository _repo;
    private readonly StoreCatalogService _service;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _uow;

    public AddOfferingHandler(
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

    public async Task Handle(AddOfferingCommand command, CancellationToken ct)
    {
        var catalog = await _repo.GetAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerException("Store catalog not found.");

        var staff = await _repo.GetStaffAsync(command.StoreId, ct);

        _service.AddOffering(catalog, staff, _currentUser.UserId, command.Name, command.Price, command.Duration, command.Description);

        await _uow.SaveChangesAsync(ct);
    }
}