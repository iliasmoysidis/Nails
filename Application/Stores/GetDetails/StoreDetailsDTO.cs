namespace Application.Stores.GetDetails;

public sealed record StoreDetailsDTO(
    int Id,
    string Name,
    string Email,
    string Phone,
    string Address,
    string TaxId
);
