
using Domain.Services;
using Application.Exceptions;
using Application.Abstractions;
using Application.Repositories;

namespace Application.UseCases.Appointment.Commands.CancelAppointment;

public sealed class CancelAppointmentHandler : ICommandHandler<CancelAppointmentCommand>
{
    private readonly IAppointmentWriteRepository _repo;
    private readonly AppointmentService _bookingService;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _uow;

    public CancelAppointmentHandler(
        IAppointmentWriteRepository repo,
        AppointmentService bookingService,
        ICurrentUser currentUser,
        IUnitOfWork uow
    )
    {
        _repo = repo;
        _bookingService = bookingService;
        _currentUser = currentUser;
        _uow = uow;
    }

    public async Task Handle(
        CancelAppointmentCommand command,
        CancellationToken ct
    )
    {
        var appointment = await _repo.GetAppointmentAsync(
            command.AppointmentId,
            ct);

        if (appointment is null)
            throw new ApplicationLayerException("Appointment not found");

        var ctx = await _repo.LoadContextAsync(
            appointment.StoreId,
            appointment.ProfessionalId,
            ct
        );

        _bookingService.CancelAppointment(
            ctx,
            appointment,
            agentId: _currentUser.UserId,
            reason: command.Reason
        );

        await _uow.SaveChangesAsync(ct);
    }
}