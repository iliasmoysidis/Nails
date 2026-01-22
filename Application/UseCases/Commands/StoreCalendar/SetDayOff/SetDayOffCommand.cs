using Application.Abstractions;

namespace Application.UseCases.Commands.StoreCalendar.SetDayOff;

public sealed record SetDayOffCommand(int StoreId,
DayOfWeek Day) : ICommand;