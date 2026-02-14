using Application.Abstractions.Policies.ProfessionalOfferings;
using Application.Abstractions.Repositories;
using Application.Abstractions.UnitOfWork;
using Application.Exceptions;

namespace Application.Commands.ProfessionalOfferings;

public sealed class AssignHandler
{
    private readonly IAssignOfferingPolicy _policy;
    private readonly IProfessionalOfferingsRepository _repo;
    private readonly IUnitOfWork _uow;

    public AssignHandler(
        IAssignOfferingPolicy policy,
        IProfessionalOfferingsRepository repo,
        IUnitOfWork uow
    )
    {
        _policy = policy;
        _repo = repo;
        _uow = uow;
    }

    public async Task Handle(AssignCommand command, CancellationToken ct)
    {
        await _policy.EnsureCanAssignOfferingAsync(command.StoreId, ct);

        var professionalOfferings = await _repo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Professional offerings not found.");

        foreach (var offeringId in command.OfferingIds)
        {
            professionalOfferings.Assign(command.ProfessionalId, offeringId);
        }

        await _uow.SaveChangesAsync(ct);
    }
}