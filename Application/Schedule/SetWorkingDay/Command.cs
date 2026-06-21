using Application.Common.DTO;
using MediatR;

namespace Application.Schedule.SetWorkingDay;

public sealed record Command(
    int StoreId,
    int ProfessionalId,
    DayOfWeek Day,
    IReadOnlyCollection<TimeRangeDTO> TimeRanges
) : IRequest;