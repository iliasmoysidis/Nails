using Application.Abstractions.Policies.Appointments;
using Application.Abstractions.Repositories;
using Application.Abstractions.UnitOfWork;
using Application.Exceptions;
using Domain.Interfaces;

namespace Application.Commands.Appointments;

public sealed class ReassignAppointmentHandler
{
    private readonly IReassignAppointmentPolicy _policy;
    private readonly IAppointmentRepository _repo;
    private readonly IClock _clock;
    private readonly IUnitOfWork _uow;

    public ReassignAppointmentHandler(
        IReassignAppointmentPolicy policy,
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

    public async Task Handle(ReassignAppointmentCommand command, CancellationToken ct)
    {
        await _policy.EnsureCanReassignAsync(command, ct);

        var appointment = await _repo.GetByIdAsync(command.AppointmentId, ct)
            ?? throw new ApplicationLayerNotFoundException("Appointment not found.");

        appointment.Reassign(command.ProfessionalId, _clock);

        await _uow.SaveChangesAsync(ct);
    }
}