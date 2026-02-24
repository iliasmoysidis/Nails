using Application.Abstractions.Policies.Professionals;
using Application.Abstractions.Repositories;
using Application.Abstractions.UnitOfWork;
using Application.Contexts;
using Application.Exceptions;
using Domain.Exceptions;
using Domain.Interfaces;

namespace Application.Commands.Professionals;

public sealed class LeaveStoreHandler
{
    private readonly IRequestContext _context;
    private readonly ILeaveStorePolicy _policy;
    private readonly IProfessionalExitService _service;
    private readonly IStaffRepository _staffRepo;
    private readonly IProfessionalOfferingsRepository _assignmentsRepo;
    private readonly IStaffCalendarRepository _calendarRepo;
    private readonly IAppointmentRepository _appointmentRepo;
    private readonly IClock _clock;
    private readonly IUnitOfWork _uow;

    public LeaveStoreHandler(
        IRequestContext context,
        ILeaveStorePolicy policy,
        IProfessionalExitService service,
        IStaffRepository staffRepo,
        IProfessionalOfferingsRepository assignmentsRepo,
        IStaffCalendarRepository calendarRepo,
        IAppointmentRepository appointmentRepo,
        IClock clock,
        IUnitOfWork uow
    )
    {
        _context = context;
        _policy = policy;
        _service = service;
        _staffRepo = staffRepo;
        _assignmentsRepo = assignmentsRepo;
        _calendarRepo = calendarRepo;
        _appointmentRepo = appointmentRepo;
        _clock = clock;
        _uow = uow;
    }

    public async Task Handle(LeaveStoreCommand command, CancellationToken ct)
    {
        await _policy.EnsureCanLeaveAsync(command.StoreId, ct);

        var upcoming = await _appointmentRepo.GetUpcomingByStoreIdAndProfessionalId(
            storeId: command.StoreId,
            professionalId: _context.ActorId,
            ct: ct
        );

        var assignments = await _assignmentsRepo.GetByStoreIdAsync(command.StoreId, ct);

        var staff = await _staffRepo.GetByStoreId(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff not found.");

        try
        {
            _service.LeaveStore(
                staff,
                assignments,
                upcoming,
                _context.ActorId,
                _clock
            );
        }
        catch (DomainException ex)
        {
            throw new ApplicationLayerValidationException(ex.Message);
        }

        await _calendarRepo.RemoveProfessionalAsync(command.StoreId, _context.ActorId, ct);

        await _uow.SaveChangesAsync(ct);
    }
}