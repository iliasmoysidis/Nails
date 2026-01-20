using Application.Abstractions;
using Domain.Entities;

namespace Application.Booking.CancelAppointment;

public sealed record CancelAppointmentCommand(
    int appointmentId
) : ICommand;