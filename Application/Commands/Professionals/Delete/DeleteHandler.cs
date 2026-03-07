using Application.Abstractions.Repositories;
using Application.Abstractions.UnitOfWork;
using Application.Guards;
using Application.Exceptions;
using Domain.Interfaces;

namespace Application.Commands.Professionals;

public sealed class DeleteHandler
{
    private readonly AuthorizationGuard _auth;
    private readonly IProfessionalRepository _repo;
    private readonly IClock _clock;
    private readonly IUnitOfWork _uow;

    public DeleteHandler(
        AuthorizationGuard auth,
        IProfessionalRepository repo,
        IClock clock,
        IUnitOfWork uow
    )
    {
        _auth = auth;
        _repo = repo;
        _clock = clock;
        _uow = uow;
    }

    public async Task Handle(DeleteCommand command, CancellationToken ct)
    {
        _auth.EnsureProfessional();
        _auth.EnsureSelf(command.ProfessionalId);

        var professional = await _repo.GetByIdAsync(command.ProfessionalId, ct)
            ?? throw new ApplicationLayerNotFoundException("Professional not found.");

        professional.SoftDelete(_clock);

        await _uow.SaveChangesAsync(ct);
    }
}