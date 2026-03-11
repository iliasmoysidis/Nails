using Application.DTO.Staff;

namespace Application.Abstractions.Queries;

public interface IStaffQueries
{
    Task<IReadOnlyCollection<StaffMemberDTO>> GetStoreStaffAsync(int storeId, CancellationToken ct);

    Task<IReadOnlyCollection<ProfessionalStoreDTO>> GetProfessionalStoresAsync(int professionalId, CancellationToken ct);
}