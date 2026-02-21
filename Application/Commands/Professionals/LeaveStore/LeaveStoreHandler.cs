using Application.Abstractions.Policies.Professionals;
using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Abstractions.UnitOfWork;
using Application.Contexts;
using Application.Exceptions;
using Domain.Interfaces;

namespace Application.Commands.Professionals;

public sealed class LeaveStoreHandler
{
    private readonly IRequestContext _context;
    private readonly ILeaveStorePolicy _policy;
    private readonly IProfessionalExitService _service;
    private readonly IUnitOfWork _uow;

    public LeaveStoreHandler(
        IRequestContext context,
        ILeaveStorePolicy policy,
        IProfessionalExitService service,
        IUnitOfWork uow
    )
    {
        _context = context;
        _policy = policy;
        _service = service;
        _uow = uow;
    }

    public async Task Handle(LeaveStoreCommand command, CancellationToken ct)
    {
        await _policy.EnsureCanLeaveAsync(command.StoreId, ct);

        await _service.LeaveStoreAsync(
            storeId: command.StoreId,
            professionalId: _context.ActorId,
            ct: ct
        );

        await _uow.SaveChangesAsync(ct);
    }
}