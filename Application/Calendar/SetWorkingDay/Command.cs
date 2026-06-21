using Application.Common.DTO;
using MediatR;

namespace Application.Calendar.SetWorkingDay;

public sealed record Command(
    int StoreId,
    DayOfWeek Day,
    IReadOnlyCollection<TimeRangeDTO> TimeRanges
) : IRequest;