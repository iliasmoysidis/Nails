namespace Application.Commands.ProfessionalOfferings;

public sealed record UnassignCommand(
    int StoreId,
    int ProfessionalId,
    IReadOnlyCollection<int> OfferingIds
);