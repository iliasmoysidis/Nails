namespace Application.Commands.StoreCalendars;

public sealed record SetStoreCalendarDayOffCommand(int StoreId, DayOfWeek Day);