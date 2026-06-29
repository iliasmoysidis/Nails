namespace Application.Assignments.GetOfferingsByProfessional;

public sealed record OfferingSummaryDTO(
    int Id,
    string Name,
    decimal Price,
    string Currency,
    int DurationMinutes
);