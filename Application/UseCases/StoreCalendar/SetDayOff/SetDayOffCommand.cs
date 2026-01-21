using Application.Abstractions;

namespace Application.UseCases.StoreCalendar.SetDayOff;

public sealed record SetDayOffCommand(int StoreId,
DayOfWeek Day) : ICommand;