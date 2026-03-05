using Application.Abstractions.Policies.Stores;
using Application.Abstractions.Repositories;
using Application.Abstractions.UnitOfWork;
using Application.Exceptions;

namespace Application.Commands.StoreCalendars;

public sealed class RemoveExceptionHandler
{
    private readonly IManageStorePolicy _policy;
    private readonly IStoreCalendarRepository _storeCalendarRepo;
    private readonly IStaffRepository _staffRepo;
    private readonly IUnitOfWork _uow;

    public RemoveExceptionHandler(
        IManageStorePolicy policy,
        IStoreCalendarRepository storeCalendarRepo,
        IStaffRepository staffRepo,
        IUnitOfWork uow
    )
    {
        _policy = policy;
        _storeCalendarRepo = storeCalendarRepo;
        _staffRepo = staffRepo;
        _uow = uow;
    }

    public async Task Handle(RemoveExceptionCommand command, CancellationToken ct)
    {
        var staff = await _staffRepo.GetByStoreId(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff not found.");

        _policy.EnsureCanManage(staff);

        var calendar = await _storeCalendarRepo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Store calendar not found");

        calendar.RemoveException(command.Date);

        await _uow.SaveChangesAsync(ct);
    }
}