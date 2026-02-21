using Application.Abstractions.Policies.Professionals;
using Application.Abstractions.Repositories;
using Application.Abstractions.UnitOfWork;
using Application.Exceptions;
using Domain.Interfaces;

namespace Application.Commands.Professionals;

public sealed class DeleteHandler
{
    private readonly IManageProfessionalPolicy _policy;
    private readonly IProfessionalRepository _repo;
    private readonly IClock _clock;
    private readonly IUnitOfWork _uow;

    public DeleteHandler(
        IManageProfessionalPolicy policy,
        IProfessionalRepository repo,
        IClock clock,
        IUnitOfWork uow
    )
    {
        _policy = policy;
        _repo = repo;
        _clock = clock;
        _uow = uow;
    }

    public async Task Handle(DeleteCommand command, CancellationToken ct)
    {
        await _policy.EnsureCanManageAsync(command.ProfessionalId, ct);

        var professional = await _repo.GetByIdAsync(command.ProfessionalId, ct)
            ?? throw new ApplicationLayerNotFoundException("Professional not found.");

        professional.SoftDelete(_clock);

        await _uow.SaveChangesAsync(ct);
    }
}