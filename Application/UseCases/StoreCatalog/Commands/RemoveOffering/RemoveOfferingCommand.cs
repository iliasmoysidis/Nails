using Application.Abstractions;

namespace Application.UseCases.StoreCatalog.Commands.RemoveOffering;

public sealed record RemoveOfferingCommand(int StoreId, int OfferingId) : ICommand;