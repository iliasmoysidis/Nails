namespace Application.Calendar.GetAvailability;

public sealed record Query(
    int StoreId,
    int ProfessionalId,
    int OfferingId,
    DateOnly Date
);