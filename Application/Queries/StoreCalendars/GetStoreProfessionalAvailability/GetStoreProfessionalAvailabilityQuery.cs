namespace Application.Queries.StoreCalendars;

public sealed record GetStoreProfessionalAvailabilityQuery(
    int StoreId,
    int ProfessionalId,
    int OfferingId,
    DateOnly Date
);