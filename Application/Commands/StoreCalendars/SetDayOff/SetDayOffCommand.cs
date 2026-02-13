namespace Application.Commands.StoreCalendars;

public sealed record SetDayOffCommand(int StoreId, DayOfWeek Day);