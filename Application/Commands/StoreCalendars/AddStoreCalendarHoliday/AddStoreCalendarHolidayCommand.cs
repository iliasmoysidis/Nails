namespace Application.Commands.StoreCalendars;

public sealed record AddStoreCalendarHolidayCommand(int StoreId, DateOnly Date);