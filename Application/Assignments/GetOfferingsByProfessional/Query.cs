namespace Application.Assignments.GetOfferingsByProfessional;

public sealed record Query(
    int StoreId,
    int ProfessionalId
);