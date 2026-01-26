
using Application.Abstractions.Repositories;
using Application.Abstractions.UnitOfWork;
using Application.Exceptions;
using Domain.Interfaces;

namespace Application.Commands.Appointments;

public sealed class ConfirmAppointmentHandler
{
    private readonly IAppointmentRepository _repo;
    private readonly IClock _clock;
    private readonly IUnitOfWork _uow;

    public ConfirmAppointmentHandler(
        IAppointmentRepository repo,
        IClock clock,
        IUnitOfWork uow
    )
    {
        _repo = repo;
        _clock = clock;
        _uow = uow;
    }

    public async Task Handle(ConfirmAppointmentCommand command, CancellationToken ct)
    {
        var appointment = await _repo.GetByIdAsync(command.AppointmentId, ct)
            ?? throw new NotFoundException("Appointment not found.");

        appointment.Confirm(_clock);

        await _uow.SaveChangesAsync(ct);
    }
}