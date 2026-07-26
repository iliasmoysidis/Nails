using Application.Common.DTO;
using Application.Roster.GetProfessionalStores;
using Application.Roster.GetStoreStaff;

namespace Application.Roster.Common.Queries;

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
