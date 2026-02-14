namespace Application.Commands.ProfessionalOfferings;

public sealed record AssignOfferingsCommand(
    int StoreId,
    int ProfessionalId,
    List<int> OfferingIds
);