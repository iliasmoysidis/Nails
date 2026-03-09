namespace Application.Commands.ProfessionalOfferings;

public sealed record UnassignProfessionalOfferingsCommand(
    int StoreId,
    int ProfessionalId,
    IReadOnlyCollection<int> OfferingIds
);