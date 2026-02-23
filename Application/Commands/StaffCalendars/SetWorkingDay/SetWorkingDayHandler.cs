using Application.Abstractions.Policies.Staffs;
using Application.Abstractions.Repositories;
using Application.Abstractions.UnitOfWork;
using Application.Abstractions.Validation.StaffCalendars;
using Application.Exceptions;
using Domain.ValueObjects.Calendar;

namespace Application.Commands.StaffCalendars;

public sealed class SetWorkingDayHandler
{
    private readonly IScheduleValidator _validator;
    private readonly IManageStaffPolicy _policy;
    private readonly IStaffCalendarRepository _repo;
    private readonly IUnitOfWork _uow;

    public SetWorkingDayHandler(
        IScheduleValidator validator,
        IManageStaffPolicy policy,
        IStaffCalendarRepository repo,
        IUnitOfWork uow
    )
    {
        _validator = validator;
        _policy = policy;
        _repo = repo;
        _uow = uow;
    }

    public async Task Handle(SetWorkingDayCommand command, CancellationToken ct)
    {
        await _validator.EnsureFitsStoreHours(command, ct);

        await _policy.EnsureCanManageStaffAsync(command.StoreId, ct);

        var calendar = await _repo.GetAsync(command.StoreId, command.ProfessionalId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff calendar not found.");

        var ranges = command.TimeRanges.Select(r => new TimeRange(r.Start, r.End));

        var workingDay = WorkingDay.WithRanges(command.Day, ranges);

        calendar.SetWorkingDay(workingDay);

        await _uow.SaveChangesAsync(ct);
    }
}