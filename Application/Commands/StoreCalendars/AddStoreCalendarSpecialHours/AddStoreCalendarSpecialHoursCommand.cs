using Application.DTO.Calendar;
using MediatR;

namespace Application.Commands.StoreCalendars;

public sealed record AddStoreCalendarSpecialHoursCommand(
    int StoreId,
    DateOnly Date,
    IReadOnlyCollection<TimeRangeDTO> TimeRanges
) : IRequest;