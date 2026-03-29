using Application.DTO.Calendar;
using MediatR;

namespace Application.Features.StoreCalendars.AddSpecialHours;

public sealed record Command(
    int StoreId,
    DateOnly Date,
    IReadOnlyCollection<TimeRangeDTO> TimeRanges
) : IRequest;