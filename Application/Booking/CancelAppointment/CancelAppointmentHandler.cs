using Application.Abstractions;
using Domain.Services.Booking;
using Application.Exceptions;

namespace Application.Booking.CancelAppointment;

public sealed class CancelAppointmentHandler : ICommandHandler<CancelAppointmentCommand>
{
    private readonly IBookingRepository _repo;
    private readonly BookingService _bookingService;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _uow;

    public CancelAppointmentHandler(
        IBookingRepository repo,
        BookingService bookingService,
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