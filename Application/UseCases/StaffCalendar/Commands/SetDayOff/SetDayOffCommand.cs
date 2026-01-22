using Application.Abstractions;

namespace Application.UseCases.StaffCalendar.Commands.SetDayOff;

public sealed record SetDayOffCommand(
    int StoreId,
    int ProfessionalId,
    DayOfWeek Day
) : ICommand;