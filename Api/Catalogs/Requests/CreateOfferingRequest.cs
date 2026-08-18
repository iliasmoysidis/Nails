namespace Api.Catalogs.Requests;

public sealed record CreateOfferingRequest(
    string Name,
    decimal Price,
    string Currency,
    int DurationMinutes,
    string? Description
);
