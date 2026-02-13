using Application.Abstractions.Policies.Stores;
using Application.Abstractions.Repositories;
using Application.Abstractions.UnitOfWork;
using Application.Exceptions;

namespace Application.Commands.StoreCalendars;

public sealed class SetDayOffHandler
{
    private readonly IManageStorePolicy _policy;
    private readonly IStoreCalendarRepository _repo;
    private readonly IUnitOfWork _uow;

    public SetDayOffHandler(
        IManageStorePolicy policy,
        IStoreCalendarRepository repo,
        IUnitOfWork uow
    )
    {
        _policy = policy;
        _repo = repo;
        _uow = uow;
    }

    public async Task Handle(SetDayOffCommand command, CancellationToken ct)
    {
        await _policy.EnsureCanManageAsync(command.StoreId, ct);

        var calendar = await _repo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Store calendar not found");

        calendar.SetDayOff(command.Day);

        await _uow.SaveChangesAsync(ct);
    }
}