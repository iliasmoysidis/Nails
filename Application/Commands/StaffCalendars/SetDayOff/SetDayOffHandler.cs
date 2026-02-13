using Application.Abstractions.Policies.Staffs;
using Application.Abstractions.Repositories;
using Application.Abstractions.UnitOfWork;
using Application.Exceptions;
using Domain.ValueObjects.Calendar;

namespace Application.Commands.StaffCalendars;

public sealed class SetDayOffHandler
{
    private readonly IManageStaffPolicy _policy;
    private readonly IStaffCalendarRepository _repo;
    private readonly IUnitOfWork _uow;

    public SetDayOffHandler(
        IManageStaffPolicy policy,
        IStaffCalendarRepository repo,
        IUnitOfWork uow
    )
    {
        _policy = policy;
        _repo = repo;
        _uow = uow;
    }

    public async Task Handle(SetDayOffCommand command, CancellationToken ct)
    {
        await _policy.EnsureCanManageStaffAsync(command.StoreId, ct);

        var calendar = await _repo.GetAsync(command.StoreId, command.ProfessionalId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff calendar not found.");

        calendar.SetDayOff(command.Day);

        await _uow.SaveChangesAsync(ct);
    }
}