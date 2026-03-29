using Application.Abstractions.Context;
using Application.Abstractions.Repositories;
using Application.Exceptions;

namespace Application.Features.StaffCalendars.SetWorkingDay;

public sealed class Loader
    : IRequestContextLoader<
        Command,
        Context>
{
    private readonly IStaffRepository _staffRepo;
    private readonly IStoreCalendarRepository _storeCalendarRepo;
    private readonly IStaffCalendarRepository _staffCalendarRepo;

    public Loader(
        IStaffRepository staffRepo,
        IStoreCalendarRepository storeCalendarRepo,
        IStaffCalendarRepository staffCalendarRepo)
    {
        _staffRepo = staffRepo;
        _storeCalendarRepo = storeCalendarRepo;
        _staffCalendarRepo = staffCalendarRepo;
    }

    public async Task PopulateAsync(
        Command command,
        Context ctx,
        CancellationToken ct)
    {
        var staff = await _staffRepo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff not found.");

        var storeCalendar = await _storeCalendarRepo.GetByIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Store calendar not found.");

        var staffCalendar = await _staffCalendarRepo.GetAsync(
            command.StoreId,
            command.ProfessionalId,
            ct)
            ?? throw new ApplicationLayerNotFoundException("Staff calendar not found.");

        ctx.Staff = staff;
        ctx.StoreCalendar = storeCalendar;
        ctx.StaffCalendar = staffCalendar;
    }
}