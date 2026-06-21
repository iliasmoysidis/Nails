namespace Application.Stores.GetSummary;

public sealed record StoreSummaryDTO(
    int Id,
    string Name,
    string City,
    string CountryCode
);