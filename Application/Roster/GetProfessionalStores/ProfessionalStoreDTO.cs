namespace Application.Roster.GetProfessionalStores;

public sealed record ProfessionalStoreDTO(
    int StoreId,
    string StoreName,
    string Address,
    bool IsOwner,
    bool IsEmployee
);
