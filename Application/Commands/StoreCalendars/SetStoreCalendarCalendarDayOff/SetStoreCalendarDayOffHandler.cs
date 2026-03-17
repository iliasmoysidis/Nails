using Application.Abstractions.Repositories;
using Application.Guards;
using Application.Exceptions;

namespace Application.Commands.StoreCalendars;

public sealed class SetStoreCalendarDayOffHandler
{
    private readonly AuthorizationGuard _auth;
    private readonly IStoreCalendarRepository _storeCalendarRepo;
    private readonly IStaffRepository _staffRepo;

    public SetStoreCalendarDayOffHandler(
        AuthorizationGuard auth,
        IStoreCalendarRepository storeCalendarRepo,
        IStaffRepository staffRepo
    )
    {
        _auth = auth;
        _storeCalendarRepo = storeCalendarRepo;
        _staffRepo = staffRepo;
    }

    public async Task Handle(SetStoreCalendarDayOffCommand command, CancellationToken ct)
    {
        var staff = await _staffRepo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff not found.");

        _auth.EnsureOwner(staff);

        var calendar = await _storeCalendarRepo.GetByIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Store calendar not found");

        calendar.SetDayOff(command.Day);
    }
}