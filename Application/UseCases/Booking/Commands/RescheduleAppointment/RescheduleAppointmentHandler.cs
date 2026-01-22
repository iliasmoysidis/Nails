using Application.Abstractions;
using Application.Exceptions;
using Application.Repositories;
using Domain.Services.Booking;

namespace Application.UseCases.Booking.Commands.RescheduleAppointment;

public sealed class RescheduleAppointmentHandler : ICommandHandler<RescheduleAppointmentCommand>
{
    private readonly IBookingWriteRepository _repo;
    private readonly BookingService _bookingService;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _uow;

    public RescheduleAppointmentHandler(
        IBookingWriteRepository repo,
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
        RescheduleAppointmentCommand command,
        CancellationToken ct
    )
    {
        var appointment = await _repo.GetAppointmentAsync(
            command.AppointmentId, ct)
            ?? throw new ApplicationLayerException("Appointment not found.");

        var ctx = await _repo.LoadContextAsync(
            appointment.StoreId,
            appointment.ProfessionalId,
            ct);

        _bookingService.RescheduleAppointment(
            ctx,
            appointment,
            _currentUser.UserId,
            command.ProfessionalId,
            command.NewStartAt
        );

        await _uow.SaveChangesAsync(ct);
    }
}