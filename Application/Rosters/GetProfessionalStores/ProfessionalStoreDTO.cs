namespace Application.Rosters.GetProfessionalStores;

public sealed record ProfessionalStoreDTO(
    int StoreId,
    string StoreName,
    string Address,
    bool IsOwner,
    bool IsEmployee
);
