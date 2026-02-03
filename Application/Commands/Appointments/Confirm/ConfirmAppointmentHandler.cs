
using Application.Abstractions.Policies.Appointments;
using Application.Abstractions.Repositories;
using Application.Abstractions.UnitOfWork;
using Application.Exceptions;
using Domain.Interfaces;

namespace Application.Commands.Appointments;

public sealed class ConfirmAppointmentHandler
{
    private readonly IConfirmAppointmentPolicy _policy;
    private readonly IAppointmentRepository _repo;
    private readonly IClock _clock;
    private readonly IUnitOfWork _uow;

    public ConfirmAppointmentHandler(
        IConfirmAppointmentPolicy policy,
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

    public async Task Handle(ConfirmAppointmentCommand command, CancellationToken ct)
    {
        await _policy.EnsureCanConfirmAsync(command, ct);

        var appointment = await _repo.GetByIdAsync(command.AppointmentId, ct)
            ?? throw new ApplicationLayerNotFoundException("Appointment not found.");

        appointment.Confirm(_clock);

        await _uow.SaveChangesAsync(ct);
    }
}