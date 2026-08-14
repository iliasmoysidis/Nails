namespace Application.Calendar.GetAvailability;

public sealed record GetCalendarAvailabilityQuery(
    int StoreId,
    int ProfessionalId,
    int OfferingId,
    DateOnly Date
);