using Application.Roster.GetProfessionalStores;
using Application.Roster.GetStoreStaff;

namespace Application.Roster.Common.Queries;

public interface IStaffQueries
{
    Task<IReadOnlyCollection<StaffMemberDTO>> GetStoreStaffAsync(int storeId, CancellationToken ct);

    Task<IReadOnlyCollection<ProfessionalStoreDTO>> GetProfessionalStoresAsync(int professionalId, CancellationToken ct);

    Task<bool> IsStaffMemberAsync(int storeId, int professionalid, CancellationToken ct);
    Task<bool> IsOwnerAsync(int storeId, int professionalid, CancellationToken ct);
}