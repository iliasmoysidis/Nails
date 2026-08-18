using Application.Catalogs.Common.DTO;
using MediatR;

namespace Application.Catalogs.GetStoreOfferings;

public sealed record GetStoreOfferingsQuery(int StoreId) : IRequest<IReadOnlyCollection<OfferingDTO>>;