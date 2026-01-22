using Application.Abstractions;

namespace Application.UseCases.StoreCatalog.Commands.UnassignOffering;

public sealed record UnassignOfferingCommand(int StoreId, int ProfessionalId, int OfferingId) : ICommand;