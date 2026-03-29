using Application.Abstractions.Repositories;
using Application.Exceptions;
using MediatR;

namespace Application.Features.Professionals.Delete;

public sealed class Handler
    : IRequestHandler<Command>
{
    private readonly IProfessionalRepository _repo;

    public Handler(IProfessionalRepository repo)
    {
        _repo = repo;
    }

    public async Task Handle(Command command, CancellationToken ct)
    {
        var isDeleted = await _repo.DeleteAsync(command.ProfessionalId, ct);

        if (!isDeleted)
            throw new ApplicationLayerNotFoundException("Professional not found.");
    }
}