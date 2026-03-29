using Application.DTO.Calendar;
using MediatR;

namespace Application.Features.StoreCalendars.SetWorkingDay;

public sealed record Command(
    int StoreId,
    DayOfWeek Day,
    IReadOnlyCollection<TimeRangeDTO> TimeRanges
) : IRequest;