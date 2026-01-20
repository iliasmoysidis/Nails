
using Application.Abstractions;
using Domain.ValueObjects.Time;

namespace Application.Booking.Commands;

public sealed record ScheduleAppointmentCommand(
    int StoreId,
    int OfferingId,
    int ProfessionalId,
    UtcDateTime StartAt,
    string? Notes
) : ICommand;