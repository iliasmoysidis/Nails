namespace Application.Queries.Assignments;

public sealed record GetProfessionalOfferingsQuery(
    int StoreId,
    int ProfessionalId
);