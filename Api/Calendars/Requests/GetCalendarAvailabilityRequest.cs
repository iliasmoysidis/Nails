namespace Api.Calendars.Requests;

public sealed record GetCalendarAvailabilityRequest(
    int ProfessionalId,
    int OfferingId,
    DateOnly Date
);
