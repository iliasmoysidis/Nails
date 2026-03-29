using Application.Abstractions.Context;
using Application.Abstractions.Repositories;
using Application.Exceptions;

namespace Application.Features.Professionals.Update;

public sealed class Loader
    : IRequestContextLoader<Command, Context>
{
    private readonly IProfessionalRepository _repo;

    public Loader(IProfessionalRepository repo)
    {
        _repo = repo;
    }

    public async Task PopulateAsync(
        Command command,
        Context ctx,
        CancellationToken ct)
    {
        var professional = await _repo.GetByIdAsync(command.ProfessionalId, ct)
            ?? throw new ApplicationLayerNotFoundException("Professional not found.");

        ctx.Professional = professional;
    }
}