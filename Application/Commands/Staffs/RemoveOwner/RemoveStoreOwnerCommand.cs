namespace Application.Commands.Stores;

public sealed record RemoveStoreOwnerCommand(int StoreId, int ProfessionalId);