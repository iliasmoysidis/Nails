namespace Application.Commands.ProfessionalOfferings;

public sealed record AssignProfessionalOfferingsCommand(
    int StoreId,
    int ProfessionalId,
    IReadOnlyCollection<int> OfferingIds
);