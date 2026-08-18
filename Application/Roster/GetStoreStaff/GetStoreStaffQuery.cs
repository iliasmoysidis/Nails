using Application.Common.DTO;
using MediatR;

namespace Application.Roster.GetStoreStaff;

public sealed record GetStoreStaffQuery(int StoreId, int? Page, int? Limit) : IRequest<PagedResult<StaffMemberDTO>>;
