namespace Application.DTO;

public sealed record StoreOfferingDTO(
    int OfferingId,
    string Name,
    decimal Price,
    string Currency,
    int DurationMinutes,
    string? Description
);