using Application.Abstractions.Policies;
using Application.Abstractions.Repositories;
using Application.Abstractions.UnitOfWork;
using Application.Exceptions;
using Domain.Interfaces;
using Domain.ValueObjects.Time;

namespace Application.Commands.Appointments;

public sealed class RescheduleAppointmentHandler
{
    private readonly IRescheduleAppointmentPolicy _policy;
    private readonly IAppointmentRepository _repo;
    private readonly IClock _clock;
    private readonly IUnitOfWork _uow;

    public RescheduleAppointmentHandler(
        IRescheduleAppointmentPolicy policy,
        IAppointmentRepository repo,
        IClock clock,
        IUnitOfWork uow
    )
    {
        _policy = policy;
        _repo = repo;
        _clock = clock;
        _uow = uow;
    }

    public async Task Handle(RescheduleAppointmentCommand command, CancellationToken ct)
    {
        await _policy.EnsureCanRescheduleAsync(command, ct);

        var appointment = await _repo.GetByIdAsync(command.AppointmentId, ct)
            ?? throw new ApplicationLayerNotFoundException("Appointment not found.");

        appointment.Reschedule(
            UtcDateTime.FromUtc(command.NewStartAt),
            _clock);

        await _uow.SaveChangesAsync(ct);
    }
}