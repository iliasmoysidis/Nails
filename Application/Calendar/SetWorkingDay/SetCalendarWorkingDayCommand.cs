using Application.Common.DTO;
using MediatR;

namespace Application.Calendar.SetWorkingDay;

public sealed record SetCalendarWorkingDayCommand(
    int StoreId,
    DayOfWeek Day,
    IReadOnlyCollection<TimeRangeDTO> TimeRanges
) : IRequest;