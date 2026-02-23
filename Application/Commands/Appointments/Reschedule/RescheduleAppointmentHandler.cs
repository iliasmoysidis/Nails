using Application.Abstractions.Policies.Appointments;
using Application.Abstractions.Repositories;
using Application.Abstractions.UnitOfWork;
using Application.Abstractions.Validation.Appointments;
using Application.Exceptions;
using Domain.Interfaces;
using Domain.ValueObjects.Time;

namespace Application.Commands.Appointments;

public sealed class RescheduleAppointmentHandler
{
    private readonly IRescheduleValidator _validator;
    private readonly IRescheduleAppointmentPolicy _policy;
    private readonly IAppointmentRepository _repo;
    private readonly IClock _clock;
    private readonly IUnitOfWork _uow;

    public RescheduleAppointmentHandler(
        IRescheduleValidator validator,
        IRescheduleAppointmentPolicy policy,
        IAppointmentRepository repo,
        IClock clock,
        IUnitOfWork uow
    )
    {
        _validator = validator;
        _policy = policy;
        _repo = repo;
        _clock = clock;
        _uow = uow;
    }

    public async Task Handle(RescheduleAppointmentCommand command, CancellationToken ct)
    {
        await _validator.EnsureAvailableAsync(command, ct);

        await _policy.EnsureCanRescheduleAsync(command, ct);

        var appointment = await _repo.GetByIdAsync(command.AppointmentId, ct)
            ?? throw new ApplicationLayerNotFoundException("Appointment not found.");

        var newStartAt = UtcDateTime.FromUtc(command.NewStartAt);

        appointment.Reschedule(newStartAt, _clock);

        await _uow.SaveChangesAsync(ct);
    }
}