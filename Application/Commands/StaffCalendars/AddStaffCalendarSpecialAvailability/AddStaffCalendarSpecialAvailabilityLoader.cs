using Application.Abstractions.Repositories;
using Application.Exceptions;

namespace Application.Commands.StaffCalendars;

public sealed class AddStaffCalendarSpecialAvailabilityLoader
    : IRequestContextLoader<
        AddStaffCalendarSpecialAvailabilityCommand,
        AddStaffCalendarSpecialAvailabilityContext>
{
    private readonly IStoreCalendarRepository _storeCalendarRepo;
    private readonly IStaffCalendarRepository _staffCalendarRepo;
    private readonly IStaffRepository _staffRepo;

    public AddStaffCalendarSpecialAvailabilityLoader(
        IStoreCalendarRepository storeCalendarRepo,
        IStaffCalendarRepository staffCalendarRepo,
        IStaffRepository staffRepo)
    {
        _storeCalendarRepo = storeCalendarRepo;
        _staffCalendarRepo = staffCalendarRepo;
        _staffRepo = staffRepo;
    }

    public async Task PopulateAsync(
        AddStaffCalendarSpecialAvailabilityCommand command,
        AddStaffCalendarSpecialAvailabilityContext ctx,
        CancellationToken ct)
    {
        var storeCalendar = await _storeCalendarRepo.GetByIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Store calendar not found.");

        var staffCalendar = await _staffCalendarRepo.GetAsync(command.StoreId, command.ProfessionalId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff calendar not found.");

        var staff = await _staffRepo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff not found.");

        ctx.StoreCalendar = storeCalendar;
        ctx.StaffCalendar = staffCalendar;
        ctx.Staff = staff;
    }
}