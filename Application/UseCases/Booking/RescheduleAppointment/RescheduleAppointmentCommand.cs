
using Application.Abstractions;
using Domain.ValueObjects.Time;

namespace Application.UseCases.Booking.RescheduleAppointment;

public sealed record RescheduleAppointmentCommand(
    int AppointmentId,
    int ProfessionalId,
    UtcDateTime NewStartAt
) : ICommand;