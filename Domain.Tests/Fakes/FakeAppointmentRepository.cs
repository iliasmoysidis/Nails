using Domain.Entities;
using Domain.Repositories;
using Domain.ValueObjects.Time;

namespace Fakes;

public sealed class FakeAppointmentRepository : IAppointmentRepository
{
    private readonly IReadOnlyCollection<Appointment> _appointments;

    public FakeAppointmentRepository(IEnumerable<Appointment> appointments)
    {
        _appointments = appointments.ToList();
    }

    public Task AddAsync(Appointment appointment)
        => Task.CompletedTask;

    public Task UpdateAsync(Appointment appointment)
        => Task.CompletedTask;

    public Task<Appointment?> GetByIdAsync(int appointmentId)
        => Task.FromResult<Appointment?>(null);

    public Task<IReadOnlyCollection<Appointment>> GetByProfessionalAsync(int professionalId, UtcDateTime? date = null)
        => Task.FromResult(_appointments);

    public Task<IReadOnlyCollection<Appointment>> GetByStoreAsync(int storeId, UtcDateTime? date = null)
        => Task.FromResult<IReadOnlyCollection<Appointment>>(Array.Empty<Appointment>());

    public Task<IReadOnlyCollection<Appointment>> GetByUserAsync(int storeId, UtcDateTime? date = null)
        => Task.FromResult<IReadOnlyCollection<Appointment>>(Array.Empty<Appointment>());
}