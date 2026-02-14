namespace Application.Commands.ProfessionalOfferings;

public sealed record AssignCommand(
    int StoreId,
    int ProfessionalId,
    List<int> OfferingIds
);