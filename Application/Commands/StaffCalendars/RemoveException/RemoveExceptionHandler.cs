using Application.Abstractions.Repositories;
using Application.Abstractions.UnitOfWork;
using Application.Guards;
using Application.Exceptions;

namespace Application.Commands.StaffCalendars;

public sealed class RemoveExceptionHandler
{
    private readonly AuthorizationGuard _auth;
    private readonly IStaffCalendarRepository _staffCalendarRepo;
    private readonly IStaffRepository _staffRepo;
    private readonly IUnitOfWork _uow;

    public RemoveExceptionHandler(
        AuthorizationGuard auth,
        IStaffCalendarRepository staffCalendarRepo,
        IStaffRepository staffRepo,
        IUnitOfWork uow
    )
    {
        _auth = auth;
        _staffCalendarRepo = staffCalendarRepo;
        _staffRepo = staffRepo;
        _uow = uow;
    }

    public async Task Handle(RemoveExceptionCommand command, CancellationToken ct)
    {
        var staff = await _staffRepo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff not found.");

        _auth.EnsureOwner(staff);

        var calendar = await _staffCalendarRepo.GetAsync(command.StoreId, command.ProfessionalId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff calendar not found.");

        calendar.RemoveException(command.Date);

        await _uow.SaveChangesAsync(ct);
    }
}