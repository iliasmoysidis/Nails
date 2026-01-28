using Application.Abstractions.Policies;
using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Abstractions.UnitOfWork;
using Application.Exceptions;
using Domain.Interfaces;
using Domain.ValueObjects.Time;

namespace Application.Commands.Appointments;

public sealed class RescheduleAppointmentHandler
{
    private readonly IAppointmentAvailabilityService _availability;
    private readonly IRescheduleAppointmentPolicy _policy;
    private readonly IAppointmentRepository _repo;
    private readonly IClock _clock;
    private readonly IUnitOfWork _uow;

    public RescheduleAppointmentHandler(
        IAppointmentAvailabilityService availability,
        IRescheduleAppointmentPolicy policy,
        IAppointmentRepository repo,
        IClock clock,
        IUnitOfWork uow
    )
    {
        _availability = availability;
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

        var newStartAt = UtcDateTime.FromUtc(command.NewStartAt);
        var newEndAt = newStartAt.Add(appointment.Duration.Value);

        await _availability.EnsureAvailableAsync(
            storeId: appointment.StoreId,
            professionalId: appointment.ProfessionalId,
            startAt: newStartAt,
            endAt: newEndAt,
            ignoreAppointmentId: appointment.Id,
            ct: ct);

        appointment.Reschedule(newStartAt, _clock);

        await _uow.SaveChangesAsync(ct);
    }
}