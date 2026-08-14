using MediatR;

namespace Application.Calendar.AddHoliday;

public sealed record AddHolidayCommand(
    int StoreId,
    DateOnly Date
) : IRequest;