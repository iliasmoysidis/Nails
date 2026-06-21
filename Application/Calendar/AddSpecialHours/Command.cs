using Application.Common.DTO;
using MediatR;

namespace Application.Calendar.AddSpecialHours;

public sealed record Command(
    int StoreId,
    DateOnly Date,
    IReadOnlyCollection<TimeRangeDTO> TimeRanges
) : IRequest;