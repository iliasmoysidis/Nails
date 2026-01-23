using Application.Abstractions;
using Application.DTO;

namespace Application.UseCases.StoreCatalog.Queries.GetStoreOfferings;

public sealed record GetStoreOfferingsQuery(int StoreId) : IQuery<IReadOnlyCollection<OfferingDetailsDTO>>;