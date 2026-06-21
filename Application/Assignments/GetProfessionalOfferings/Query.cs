namespace Application.Assignments.GetProfessionalOfferings;

public sealed record Query(
    int StoreId,
    int ProfessionalId
);