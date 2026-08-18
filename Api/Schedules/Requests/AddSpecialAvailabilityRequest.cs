using Application.Common.DTO;

namespace Api.Schedules.Requests;

public sealed record AddSpecialAvailabilityRequest(
    DateOnly Date,
    IReadOnlyCollection<TimeRangeDTO> TimeRanges
);
