namespace Application.Commands.Offerings;

public sealed record DeleteOfferingCommand(int StoreId, int OfferingId);