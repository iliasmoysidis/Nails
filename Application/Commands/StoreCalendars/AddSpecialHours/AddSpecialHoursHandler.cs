using Application.Abstractions.Policies.Stores;
using Application.Abstractions.Repositories;
using Application.Abstractions.UnitOfWork;
using Application.Exceptions;
using Domain.ValueObjects.Calendar;

namespace Application.Commands.StoreCalendars;

public sealed class AddSpecialHoursHandler
{
    private readonly IManageStorePolicy _policy;
    private readonly IStoreCalendarRepository _repo;
    private readonly IUnitOfWork _uow;

    public AddSpecialHoursHandler(
        IManageStorePolicy policy,
        IStoreCalendarRepository repo,
        IUnitOfWork uow
    )
    {
        _policy = policy;
        _repo = repo;
        _uow = uow;
    }

    public async Task Handle(AddSpecialHoursCommand command, CancellationToken ct)
    {
        await _policy.EnsureCanManageAsync(command.StoreId, ct);

        var calendar = await _repo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Store calendar not found");

        var ranges = command.TimeRanges
            .Select(r => new TimeRange(r.Start, r.End))
            .ToList();

        var exception = CalendarException.PartialDay(command.Date, ranges);

        calendar.AddException(exception);

        await _uow.SaveChangesAsync(ct);
    }
}