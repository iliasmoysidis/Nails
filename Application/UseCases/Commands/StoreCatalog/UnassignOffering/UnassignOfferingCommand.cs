using Application.Abstractions;

namespace Application.UseCases.Commands.StoreCatalog.UnassignOffering;

public sealed record UnassignOfferingCommand(int StoreId, int ProfessionalId, int OfferingId) : ICommand;