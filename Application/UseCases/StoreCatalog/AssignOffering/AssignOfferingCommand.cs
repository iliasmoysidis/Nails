
using Application.Abstractions;

namespace Application.UseCases.StoreCatalog.AssignOffering;

public sealed record AssignOfferingCommand(int StoreId, int ProfessionalId, int OfferingId) : ICommand;