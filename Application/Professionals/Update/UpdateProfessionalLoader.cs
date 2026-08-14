using Application.Professionals.Common.Repositories;
using Application.Common.Abstractions.Context;
using Application.Common.Exceptions;

namespace Application.Professionals.Update;

public sealed class UpdateProfessionalLoader
    : IRequestContextLoader<UpdateProfessionalCommand, UpdateProfessionalContext>
{
    private readonly IProfessionalRepository _repo;

    public UpdateProfessionalLoader(IProfessionalRepository repo)
    {
        _repo = repo;
    }

    public async Task PopulateAsync(
        UpdateProfessionalCommand command,
        UpdateProfessionalContext ctx,
        CancellationToken ct)
    {
        var professional = await _repo.GetByIdAsync(command.ProfessionalId, ct)
            ?? throw new ApplicationLayerNotFoundException("Professional not found.");

        ctx.Professional = professional;
    }
}