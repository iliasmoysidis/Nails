using Application.Common.DTO;
using Application.Stores.Common.DTO;
using MediatR;

namespace Application.Stores.GetAll;

public sealed record GetAllStoresQuery(int? Page, int? Limit) : IRequest<PagedResult<StoreListItemDTO>>;
