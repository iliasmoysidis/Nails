using Application.Abstractions;
using Domain.ValueObjects.Calendar;

namespace Application.UseCases.StaffCalendar.Commands.AddException;

public sealed record AddExceptionCommand(
    int StoreId,
    int ProfessionalId,
    DateOnly Date,
    bool IsDayOff,
    IReadOnlyCollection<TimeRange> TimeRanges
) : ICommand;