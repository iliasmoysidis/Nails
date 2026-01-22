using Application.Abstractions;
using Application.Repositories;
using Domain.Services.Booking;

namespace Application.UseCases.Booking.Commands.ScheduleAppointment;

public sealed class ScheduleAppointmentCommandHandler : ICommandHandler<ScheduleAppointmentCommand>
{
    private readonly IBookingWriteRepository _repo;
    private readonly BookingService _bookingService;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _uow;

    public ScheduleAppointmentCommandHandler(
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
        ScheduleAppointmentCommand command,
        CancellationToken ct)
    {
        var ctx = await _repo.LoadContextAsync(
            command.StoreId,
            command.ProfessionalId,
            ct
        );

        var appointment = _bookingService.ScheduleAppointment(
            ctx,
            userId: _currentUser.UserId,
            offeringId: command.OfferingId,
            professionalId: command.ProfessionalId,
            storeId: command.StoreId,
            startAt: command.StartAt,
            notes: command.Notes
        );

        _repo.Add(appointment);

        await _uow.SaveChangesAsync(ct);
    }

}