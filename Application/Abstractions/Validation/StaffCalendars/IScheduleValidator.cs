using Application.Abstractions.Repositories;
using Application.Commands.StaffCalendars;
using Domain.Entities;
using Domain.ValueObjects.Calendar;

namespace Application.Abstractions.Validation.StaffCalendars;

public interface IScheduleValidator
{
    Task EnsureFitsStoreHours(SetWorkingDayCommand command, CancellationToken ct);

    Task EnsureExceptionFitsStoreHours(AddSpecialAvailabilityCommand command, CancellationToken ct);
}