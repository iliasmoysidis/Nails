using Application.Roster.Common.Queries;
using Application.Roster.GetProfessionalStores;
using Application.Roster.GetStoreStaff;
using Domain.Roster.EnumObjects;
using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Roster;

public sealed class StaffQueries : IStaffQueries
{
    private readonly AppDbContext _context;

    public StaffQueries(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyCollection<ProfessionalStoreDTO>> GetProfessionalStoresAsync(int professionalId, CancellationToken ct)
    {
        return await (
            from staff in _context.Staff

            from member in staff.Members
            where member.ProfessionalId == professionalId

            join store in _context.Stores
                on staff.StoreId equals store.Id

            orderby store.Name.ToString()

            select new ProfessionalStoreDTO(
                store.Id,
                store.Name.ToString(),
                store.Address.ToString(),
                member.Roles.Any(r => r.Role == StaffRole.Owner),
                member.Roles.Any(r => r.Role == StaffRole.Employee)
            )
        ).ToListAsync(ct);
    }

    public async Task<IReadOnlyCollection<StaffMemberDTO>> GetStoreStaffAsync(int storeId, CancellationToken ct)
    {
        return await (
            from staff in _context.Staff
            where staff.StoreId == storeId

            from member in staff.Members

            join professional in _context.Professionals
                on member.ProfessionalId equals professional.Id

            orderby professional.FullName.ToString()

            select new StaffMemberDTO(
                professional.Id,
                professional.FullName.ToString(),
                professional.Email.ToString(),
                professional.Phone.ToString(),
                member.Roles.Any(r => r.Role == StaffRole.Owner),
                member.Roles.Any(r => r.Role == StaffRole.Employee)
            )
        ).ToListAsync(ct);
    }

    public Task<bool> IsOwnerAsync(int storeId, int professionalid, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsStaffMemberAsync(int storeId, int professionalid, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}
