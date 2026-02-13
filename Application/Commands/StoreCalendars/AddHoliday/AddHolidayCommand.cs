namespace Application.Commands.StoreCalendars;

public sealed record AddHolidayCommand(int StoreId, DateOnly Date);