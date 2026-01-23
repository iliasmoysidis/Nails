namespace Application.DTO;

public sealed record OfferingDetailsDTO(
    int OfferingId,
    string Name,
    decimal Price,
    string Currency,
    int DurationMinutes,
    string? Description
);