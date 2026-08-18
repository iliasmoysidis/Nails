using Application.Common.DTO;

namespace Api.Calendars.Requests;

public sealed record AddSpecialHoursRequest(
    DateOnly Date,
    IReadOnlyCollection<TimeRangeDTO> TimeRanges
);
