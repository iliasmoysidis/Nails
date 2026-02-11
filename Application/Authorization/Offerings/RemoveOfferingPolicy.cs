using Application.Abstractions.Policies.Offerings;
using Application.Abstractions.Repositories;
using Application.Commands.Offerings;
using Application.Contexts;
using Application.Exceptions;

namespace Application.Authorization.Offerings;

public sealed class RemoveOfferingPolicy : IRemoveOfferingPolicy
{
    private readonly IRequestContext _context;
    private readonly IStaffRepository _repo;

    public RemoveOfferingPolicy(IRequestContext context, IStaffRepository repo)
    {
        _context = context;
        _repo = repo;
    }

    public async Task EnsureCanRemoveAsync(RemoveOfferingCommand command, CancellationToken ct)
    {
        if (!_context.IsProfessional)
            throw Forbidden();

        var staff = await _repo.GetByStoreId(command.StoreId, ct)
            ?? throw Forbidden();

        if (!staff.IsOwner(_context.ActorId))
            throw Forbidden();
    }

    private static ApplicationLayerForbiddenException Forbidden()
        => new("Only store owners can remove offerings.");
}