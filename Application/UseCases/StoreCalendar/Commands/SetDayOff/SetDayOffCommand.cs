using Application.Abstractions;

namespace Application.UseCases.StoreCalendar.Commands.SetDayOff;

public sealed record SetDayOffCommand(int StoreId,
DayOfWeek Day) : ICommand;