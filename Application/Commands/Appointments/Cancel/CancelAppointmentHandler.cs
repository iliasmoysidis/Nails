using Application.Abstractions.Policies;
using Application.Abstractions.Repositories;
using Application.Abstractions.UnitOfWork;
using Application.Exceptions;
using Domain.Interfaces;

namespace Application.Commands.Appointments;

public sealed class CancelAppointmentHandler
{
    private readonly ICancelAppointmentPolicy _policy;
    private readonly IAppointmentRepository _repo;
    private readonly IClock _clock;
    private readonly IUnitOfWork _uow;

    public CancelAppointmentHandler(
        ICancelAppointmentPolicy policy,
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

    public async Task Handle(CancelAppointmentCommand command, CancellationToken ct)
    {
        await _policy.EnsureCanCancelAsync(command, ct);

        var appointment = await _repo.GetByIdAsync(command.AppointmentId, ct)
            ?? throw new ApplicationLayerNotFoundException("Appointment not found.");

        appointment.Cancel(_clock, command.Reason);
        await _uow.SaveChangesAsync(ct);
    }
}