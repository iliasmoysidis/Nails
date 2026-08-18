using Application.Common.DTO;
using Application.Rosters.GetProfessionalStores;
using Application.Rosters.GetStoreStaff;

namespace Application.Rosters.Common.Queries;

public interface IStaffQueries
{
    Task<PagedResult<StaffMemberDTO>> GetStoreStaffAsync(
        int storeId,
        int? page,
        int? limit,
        CancellationToken ct
    );

    Task<IReadOnlyCollection<ProfessionalStoreDTO>> GetProfessionalStoresAsync(int professionalId, CancellationToken ct);
}
