using Application.Abstractions.Repositories;
using Application.Exceptions;

namespace Application.Commands.StaffCalendars;

public sealed class SetStaffCalendarDayOffLoader
    : IRequestContextLoader<
        SetStaffCalendarDayOffCommand,
        SetStaffCalendarDayOffContext>
{
    private readonly IStaffRepository _staffRepo;
    private readonly IStaffCalendarRepository _staffCalendarRepo;

    public SetStaffCalendarDayOffLoader(
        IStaffRepository staffRepo,
        IStaffCalendarRepository staffCalendarRepo)
    {
        _staffRepo = staffRepo;
        _staffCalendarRepo = staffCalendarRepo;
    }

    public async Task PopulateAsync(
        SetStaffCalendarDayOffCommand command,
        SetStaffCalendarDayOffContext ctx,
        CancellationToken ct)
    {
        var staff = await _staffRepo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff not found.");

        var calendar = await _staffCalendarRepo.GetAsync(
            command.StoreId,
            command.ProfessionalId,
            ct)
            ?? throw new ApplicationLayerNotFoundException("Staff calendar not found.");

        ctx.Staff = staff;
        ctx.StaffCalendar = calendar;
    }
}