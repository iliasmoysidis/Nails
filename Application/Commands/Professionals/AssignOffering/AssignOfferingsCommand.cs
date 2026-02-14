namespace Application.Commands.Professionals;

public sealed record AssignOfferingsCommand(
    int StoreId,
    int ProfessionalId,
    List<int> OfferingIds
);