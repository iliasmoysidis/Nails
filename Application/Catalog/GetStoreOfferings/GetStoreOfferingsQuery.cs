using Application.Catalog.Common.DTO;
using MediatR;

namespace Application.Catalog.GetStoreOfferings;

public sealed record GetStoreOfferingsQuery(int StoreId) : IRequest<IReadOnlyCollection<OfferingDTO>>;