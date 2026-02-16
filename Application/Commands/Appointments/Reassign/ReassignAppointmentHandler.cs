using Application.Abstractions.Policies.Appointments;
using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Abstractions.UnitOfWork;
using Application.Exceptions;
using Domain.Interfaces;

namespace Application.Commands.Appointments;

public sealed class ReassignAppointmentHandler
{
    private readonly IReassignAppointmentPolicy _policy;
    private readonly IAppointmentAvailabilityService _availability;
    private readonly IAppointmentRepository _repo;
    private readonly IClock _clock;
    private readonly IUnitOfWork _uow;

    public ReassignAppointmentHandler(
        IReassignAppointmentPolicy policy,
        IAppointmentAvailabilityService availability,
        IAppointmentRepository repo,
        IClock clock,
        IUnitOfWork uow
    )
    {
        _policy = policy;
        _availability = availability;
        _repo = repo;
        _clock = clock;
        _uow = uow;
    }

    public async Task Handle(ReassignAppointmentCommand command, CancellationToken ct)
    {
        await _policy.EnsureCanReassignAsync(command, ct);

        var appointment = await _repo.GetByIdAsync(command.AppointmentId, ct)
            ?? throw new ApplicationLayerNotFoundException("Appointment not found.");

        await _availability.EnsureAvailableAsync(
            storeId: appointment.StoreId,
            professionalId: command.ProfessionalId,
            startAt: appointment.StartAt,
            endAt: appointment.EndAt,
            ignoreAppointmentId: null,
            ct: ct
        );

        appointment.Reassign(command.ProfessionalId, _clock);

        await _uow.SaveChangesAsync(ct);
    }
}