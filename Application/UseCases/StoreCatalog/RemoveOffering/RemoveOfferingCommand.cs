using Application.Abstractions;

namespace Application.UseCases.StoreCatalog.RemoveOffering;

public sealed record RemoveOfferingCommand(int StoreId, int OfferingId) : ICommand;