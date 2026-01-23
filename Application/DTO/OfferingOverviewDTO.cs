namespace Application.DTO;

public sealed record OfferingOverviewDTO(
    int OfferingId,
    string Name,
    decimal Price
);