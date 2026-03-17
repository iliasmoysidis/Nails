using Application.Abstractions.Repositories;
using Application.Guards;
using Application.Exceptions;
using Domain.Interfaces;

namespace Application.Commands.Offerings;

public sealed class DeleteOfferingHandler
{
    private readonly AuthorizationGuard _auth;
    private readonly IStoreCatalogRepository _catalogRepo;
    private readonly IAssignmentsRepository _assignmentsRepo;
    private readonly IStaffRepository _staffRepo;
    private readonly IClock _clock;

    public DeleteOfferingHandler(
        AuthorizationGuard auth,
        IAssignmentsRepository assignmentsRepo,
        IStoreCatalogRepository catalogRepo,
        IStaffRepository staffRepo,
        IClock clock
    )
    {
        _auth = auth;
        _catalogRepo = catalogRepo;
        _assignmentsRepo = assignmentsRepo;
        _staffRepo = staffRepo;
        _clock = clock;
    }

    public async Task Handle(DeleteOfferingCommand command, CancellationToken ct)
    {
        var staff = await _staffRepo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff not found.");

        _auth.EnsureOwner(staff);

        var catalog = await _catalogRepo.GetByIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException($"Store catalog not found for store {command.StoreId}.");

        var assignments = await _assignmentsRepo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException($"Professional offerings not found for store {command.StoreId}.");

        assignments.RemoveOfferingAssignments(command.OfferingId);

        catalog.RemoveOffering(command.OfferingId, _clock);
    }
}