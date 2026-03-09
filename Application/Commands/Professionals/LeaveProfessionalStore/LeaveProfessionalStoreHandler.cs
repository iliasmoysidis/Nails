using Application.Abstractions.Repositories;
using Application.Abstractions.UnitOfWork;
using Application.Guards;
using Application.Exceptions;
using Domain.Exceptions;
using Domain.Interfaces;

namespace Application.Commands.Professionals;

public sealed class LeaveProfessionalStoreHandler
{
    private readonly AuthorizationGuard _auth;
    private readonly IProfessionalExitService _service;
    private readonly IStaffRepository _staffRepo;
    private readonly IAssignmentsRepository _assignmentsRepo;
    private readonly IStaffCalendarRepository _calendarRepo;
    private readonly IAppointmentRepository _appointmentRepo;
    private readonly IClock _clock;
    private readonly IUnitOfWork _uow;

    public LeaveProfessionalStoreHandler(
        AuthorizationGuard auth,
        IProfessionalExitService service,
        IStaffRepository staffRepo,
        IAssignmentsRepository assignmentsRepo,
        IStaffCalendarRepository calendarRepo,
        IAppointmentRepository appointmentRepo,
        IClock clock,
        IUnitOfWork uow
    )
    {
        _auth = auth;
        _service = service;
        _staffRepo = staffRepo;
        _assignmentsRepo = assignmentsRepo;
        _calendarRepo = calendarRepo;
        _appointmentRepo = appointmentRepo;
        _clock = clock;
        _uow = uow;
    }

    public async Task Handle(LeaveProfessionalStoreCommand command, CancellationToken ct)
    {
        var staff = await _staffRepo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff not found.");

        _auth.EnsureProfessional();
        _auth.EnsureSelf(command.ProfessionalId);
        _auth.EnsureStaffMember(staff);

        var upcoming = await _appointmentRepo.GetUpcomingByStoreIdAndProfessionalId(
            storeId: command.StoreId,
            professionalId: command.ProfessionalId,
            ct: ct
        );

        var assignments = await _assignmentsRepo.GetByStoreIdAsync(command.StoreId, ct);

        try
        {
            _service.LeaveStore(
                staff,
                assignments,
                upcoming,
                command.ProfessionalId,
                _clock
            );
        }
        catch (DomainException ex)
        {
            throw new ApplicationLayerValidationException(ex.Message);
        }

        await _calendarRepo.RemoveProfessionalAsync(command.StoreId, command.ProfessionalId, ct);

        await _uow.SaveChangesAsync(ct);
    }
}