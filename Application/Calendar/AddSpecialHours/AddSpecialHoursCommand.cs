using Application.Common.DTO;
using MediatR;

namespace Application.Calendar.AddSpecialHours;

public sealed record AddSpecialHoursCommand(
    int StoreId,
    DateOnly Date,
    IReadOnlyCollection<TimeRangeDTO> TimeRanges
) : IRequest;