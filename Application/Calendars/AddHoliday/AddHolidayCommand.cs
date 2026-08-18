using MediatR;

namespace Application.Calendars.AddHoliday;

public sealed record AddHolidayCommand(
    int StoreId,
    DateOnly Date
) : IRequest;