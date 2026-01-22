
using Application.Abstractions;
using Domain.ValueObjects.Time;

namespace Application.UseCases.Booking.Commands.RescheduleAppointment;

public sealed record RescheduleAppointmentCommand(
    int AppointmentId,
    int ProfessionalId,
    UtcDateTime NewStartAt
) : ICommand;