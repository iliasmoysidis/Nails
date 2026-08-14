namespace Application.Assignments.GetOfferingsByProfessional;

public sealed record GetOfferingsByProfessionalQuery(
    int StoreId,
    int ProfessionalId,
    int? Page,
    int? Limit
);
