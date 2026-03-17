using Application.Abstractions.Repositories;
using Application.Exceptions;

namespace Application.Commands.Professionals;

public sealed class DeleteProfessionalLoader
    : IRequestContextLoader<DeleteProfessionalCommand, DeleteProfessionalContext>
{
    private readonly IProfessionalRepository _repo;

    public DeleteProfessionalLoader(IProfessionalRepository repo)
    {
        _repo = repo;
    }

    public async Task PopulateAsync(
        DeleteProfessionalCommand command,
        DeleteProfessionalContext ctx,
        CancellationToken ct)
    {
        ctx.Professional = await _repo.GetByIdAsync(command.ProfessionalId, ct)
            ?? throw new ApplicationLayerNotFoundException("Professional not found.");
    }
}