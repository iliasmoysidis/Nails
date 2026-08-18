using MediatR;

namespace Application.Stores.GetDetails;

public sealed record GetStoreDetailsQuery(int StoreId) : IRequest<StoreDetailsDTO>;