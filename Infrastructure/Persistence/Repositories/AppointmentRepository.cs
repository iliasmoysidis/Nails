using Domain.Entities;
using Domain.Repositories;
using Domain.ValueObjects.Time;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

internal sealed class AppointmentRepository : IAppointmentRepository
{
    private readonly AppDbContext _db;

    public AppointmentRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(Appointment appointment)
    {
        _db.Appointments.Add(appointment);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(Appointment appointment)
    {
        await _db.SaveChangesAsync();
    }

    public async Task<Appointment?> GetByIdAsync(int appointmentId)
    {
        return await _db.Appointments.FirstOrDefaultAsync(a => a.Id == appointmentId);
    }

    public async Task<IReadOnlyCollection<Appointment>> GetByProfessionalAsync(int professionalId, UtcDateTime? date = null)
    {
        var query = _db.Appointments.Where(a => a.ProfessionalId == professionalId);

        if (date.HasValue)
        {
            var day = date.Value.Date;
            query = query.Where(a => a.StartAt.Date == day);
        }

        return await query.OrderBy(a => a.StartAt).ToListAsync();
    }

    public async Task<IReadOnlyCollection<Appointment>> GetByStoreAsync(int storeId, UtcDateTime? date = null)
    {
        var query = _db.Appointments.Where(a => a.StoreId == storeId);

        if (date.HasValue)
        {
            var day = date.Value.Date;
            query = query.Where(a => a.StartAt.Date == day);
        }

        return await query.OrderBy(a => a.StartAt).ToListAsync();
    }

    public async Task<IReadOnlyCollection<Appointment>> GetByUserAsync(int userId, UtcDateTime? date = null)
    {
        var query = _db.Appointments.Where(a => a.UserId == userId);

        if (date.HasValue)
        {
            var day = date.Value.Date;
            query = query.Where(a => a.StartAt.Date == day);
        }

        return await query.OrderBy(a => a.StartAt).ToListAsync();
    }
}