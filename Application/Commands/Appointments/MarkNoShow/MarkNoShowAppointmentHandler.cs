using Application.Abstractions.Policies;
using Application.Abstractions.Repositories;
using Application.Abstractions.UnitOfWork;
using Application.Exceptions;
using Domain.Interfaces;

namespace Application.Commands.Appointments;

public sealed class MarkNoShowAppointmentHandler
{
    private readonly IMarkNoShowAppointmentPolicy _policy;
    private readonly IAppointmentRepository _repo;
    private readonly IClock _clock;
    private readonly IUnitOfWork _uow;

    public MarkNoShowAppointmentHandler(
        IMarkNoShowAppointmentPolicy policy,
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

    public async Task Handle(MarkNoShowAppointmentCommand command, CancellationToken ct)
    {
        await _policy.EnsureCanMarkNoShowAsync(command, ct);

        var appointment = await _repo.GetByIdAsync(command.AppointmentId, ct)
            ?? throw new ApplicationLayerNotFoundException("Appointment not found.");

        appointment.MarkAsNoShow(_clock);

        await _uow.SaveChangesAsync(ct);
    }
}