
using Application.Abstractions;

namespace Application.UseCases.StoreCatalog.Commands.AssignOffering;

public sealed record AssignOfferingCommand(int StoreId, int ProfessionalId, int OfferingId) : ICommand;