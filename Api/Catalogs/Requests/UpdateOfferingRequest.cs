namespace Api.Catalogs.Requests;

public sealed record UpdateOfferingRequest(
    string? Name,
    decimal? Price,
    int? DurationMinutes,
    string? Description
);
