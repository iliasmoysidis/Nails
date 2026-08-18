namespace Api.Professionals.Requests;

public sealed record SearchProfessionalRequest(
    string? Name,
    string? Email,
    string? Phone,
    int? Page,
    int? Limit
);
