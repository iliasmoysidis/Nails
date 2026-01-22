using Application.Abstractions;

namespace Application.UseCases.Commands.StaffCalendar.SetDayOff;

public sealed record SetDayOffCommand(
    int StoreId,
    int ProfessionalId,
    DayOfWeek Day
) : ICommand;