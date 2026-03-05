using Application.Abstractions.Policies.Stores;
using Application.Abstractions.Repositories;
using Application.Abstractions.UnitOfWork;
using Application.Exceptions;

namespace Application.Commands.StoreCalendars;

public sealed class SetDayOffHandler
{
    private readonly IManageStorePolicy _policy;
    private readonly IStoreCalendarRepository _storeCalendarRepo;
    private readonly IStaffRepository _staffRepo;
    private readonly IUnitOfWork _uow;

    public SetDayOffHandler(
        IManageStorePolicy policy,
        IStoreCalendarRepository storeCalendarRepo,
        IStaffRepository staffRepo,
        IUnitOfWork uow
    )
    {
        _policy = policy;
        _storeCalendarRepo = storeCalendarRepo;
        _staffRepo = staffRepo;
        _uow = uow;
    }

    public async Task Handle(SetDayOffCommand command, CancellationToken ct)
    {
        var staff = await _staffRepo.GetByStoreId(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff not found.");

        _policy.EnsureCanManage(staff);

        var calendar = await _storeCalendarRepo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Store calendar not found");

        calendar.SetDayOff(command.Day);

        await _uow.SaveChangesAsync(ct);
    }
}