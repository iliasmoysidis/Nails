namespace Application.Commands.Offerings;

public sealed record RemoveOfferingCommand(int StoreId, int OfferingId);