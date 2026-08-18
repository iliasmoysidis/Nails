namespace Application.Catalogs.Common.DTO;

public sealed record OfferingDTO(
    int Id,
    string Name,
    decimal Price,
    string Currency,
    int DurationMinutes,
    string Description
);