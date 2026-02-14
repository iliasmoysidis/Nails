namespace Application.Commands.ProfessionalOfferings;

public sealed record UnassignCommand(
    int StoreId,
    int ProfessionalId,
    List<int> OfferingIds
);