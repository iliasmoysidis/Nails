using Application.Abstractions.Repositories;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public sealed class StaffRepository : IStaffRepository
{
    private readonly AppDbContext _db;

    public StaffRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(Staff staff, CancellationToken ct)
    {
        var exists = await _db.Staff.AnyAsync(x => x.StoreId == staff.StoreId, ct);

        if (exists)
            throw new InvalidOperationException("Staff already exists");

        await _db.Staff.AddAsync(new StaffEntity { StoreId = staff.StoreId }, ct);

        var members = staff.Members.Select(m => new StaffMemberEntity { StoreId = staff.StoreId, ProfessionalId = m.ProfessionalId });
        await _db.StaffMembers.AddRangeAsync(members, ct);

        var roles = staff.Members.SelectMany(m => m.Roles.Select(role => new StaffRoleEntity
        {
            StoreId = staff.StoreId,
            ProfessionalId = m.ProfessionalId,
            Role = role
        }));

        await _db.StaffRoles.AddRangeAsync(roles, ct);
    }

    public async Task<Staff?> GetByStoreIdAsync(int storeId, CancellationToken ct)
    {
        var staffExists = await _db.Staff
        .AsNoTracking()
        .AnyAsync(s => s.StoreId == storeId, ct);

        if (!staffExists)
            return null;

        var members = await _db.StaffMembers
            .AsNoTracking()
            .Where(x => x.StoreId == storeId)
            .ToListAsync(ct);

        if (!members.Any())
            return null;

        var roles = await _db.StaffRoles
            .AsNoTracking()
            .Where(x => x.StoreId == storeId)
            .ToListAsync(ct);

        var rolesByProfessional = roles
            .GroupBy(r => r.ProfessionalId)
            .ToDictionary(
                g => g.Key,
                g => g.Select(x => x.Role)
            );

        var grouped = members.Select(m => (
            m.ProfessionalId,
            Roles: rolesByProfessional.TryGetValue(m.ProfessionalId, out var r)
            ? r
            : Enumerable.Empty<StaffRole>()
        ));

        return Staff.Rehydrate(storeId, grouped);
    }
}