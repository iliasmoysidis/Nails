
using Application.Abstractions;
using Domain.ValueObjects.Time;

namespace Application.Booking.RescheduleAppointment;

public sealed record RescheduleAppointmentCommand(
    int AppointmentId,
    int ProfessionalId,
    UtcDateTime NewStartAt
) : ICommand;