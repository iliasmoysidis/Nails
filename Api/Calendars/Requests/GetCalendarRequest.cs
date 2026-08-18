namespace Api.Calendars.Requests;

public sealed record GetCalendarRequest(DateOnly From, DateOnly To);
