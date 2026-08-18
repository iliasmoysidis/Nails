namespace Api.Schedules.Requests;

public sealed record GetScheduleExceptionsRequest(
    DateOnly From,
    DateOnly To,
    int? Page,
    int? Limit
);
