using Application.Abstractions.Repositories;
using Application.Commands.StaffCalendars;
using Domain.Entities;
using Domain.ValueObjects.Calendar;

namespace Application.Abstractions.Validation.StaffCalendars;

public interface IScheduleValidator
{
    // private readonly IStoreCalendarRepository _repo;

    // public IScheduleValidator(IStoreCalendarRepository repo)
    // {
    //     _repo = repo;
    // }

    Task EnsureFitsStoreHours(SetWorkingDayCommand command, CancellationToken ct);
}