using Application.Abstractions;

namespace Application.UseCases.Commands.StoreCatalog.RemoveOffering;

public sealed record RemoveOfferingCommand(int StoreId, int OfferingId) : ICommand;