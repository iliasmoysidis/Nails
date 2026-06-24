using Application.Appointments.Common.Repositories;
using Infrastructure.Common;
using Domain.Appointments;
using Domain.Appointments.EnumObjects;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Appointments;

public sealed class AppointmentRepository : IAppointmentRepository
{
    private readonly AppDbContext _db;

    public AppointmentRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Appointment?> GetByIdAsync(int id, CancellationToken ct)
        => await _db.Appointments.FindAsync([id], ct);

    public async Task<IReadOnlyCollection<Appointment>> GetByProfessionalIdAsync(int professionalId, CancellationToken ct)
    {
        return await _db.Appointments.Where(a => a.ProfessionalId == professionalId).ToListAsync(ct);
    }

    public async Task<IReadOnlyCollection<Appointment>> GetByUserIdAsync(int userId, CancellationToken ct)
    {
        return await _db.Appointments.Where(a => a.UserId == userId).ToListAsync(ct);
    }

    public async Task<IReadOnlyCollection<Appointment>> GetUpcomingByStoreIdAsync(int storeId, CancellationToken ct)
    {
        return await _db.Appointments.Where(a => a.StoreId == storeId && a.Status == AppointmentStatus.Confirmed).ToListAsync(ct);
    }

    public async Task<IReadOnlyCollection<Appointment>> GetUpcomingByStoreIdAndProfessionalId(int storeId, int professionalId, CancellationToken ct)
    {
        return await _db.Appointments.Where(a => a.StoreId == storeId && a.ProfessionalId == professionalId && a.Status == AppointmentStatus.Confirmed).ToListAsync(ct);
    }

    public async Task<IReadOnlyCollection<Appointment>> GetUpcomingByProfessionalIdAsync(int professionalId, CancellationToken ct)
    {
        return await _db.Appointments.Where(a => a.ProfessionalId == professionalId && a.Status == AppointmentStatus.Confirmed).ToListAsync(ct);
    }

    public async Task<IReadOnlyCollection<Appointment>> GetUpcomingByUserIdAsync(int userId, CancellationToken ct)
    {
        return await _db.Appointments.Where(a => a.UserId == userId && a.Status == AppointmentStatus.Confirmed).ToListAsync(ct);
    }

    public async Task AddAsync(Appointment appointment, CancellationToken ct)
    {
        await _db.Appointments.AddAsync(appointment);
    }
}