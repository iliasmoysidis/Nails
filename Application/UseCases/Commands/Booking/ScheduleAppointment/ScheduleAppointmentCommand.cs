
using Application.Abstractions;
using Domain.ValueObjects.Time;

namespace Application.UseCases.Commands.Booking.ScheduleAppointment;

public sealed record ScheduleAppointmentCommand(
    int StoreId,
    int OfferingId,
    int ProfessionalId,
    UtcDateTime StartAt,
    string? Notes
) : ICommand;