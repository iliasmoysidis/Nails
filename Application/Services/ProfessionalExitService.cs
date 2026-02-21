using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Exceptions;
using Domain.Interfaces;

namespace Application.Services;

public sealed class ProfessionalExitService : IProfessionalExitService
{
    private readonly IStaffRepository _staffRepo;
    private readonly IProfessionalOfferingsRepository _assignmentsRepo;
    private readonly IStaffCalendarRepository _calendarRepo;
    private readonly IAppointmentRepository _appointmentRepo;
    private readonly IClock _clock;

    public ProfessionalExitService(
        IStaffRepository staffRepo,
        IProfessionalOfferingsRepository assignmentsRepo,
        IStaffCalendarRepository calendarRepo,
        IAppointmentRepository appointmentRepo,
        IClock clock)
    {
        _staffRepo = staffRepo;
        _assignmentsRepo = assignmentsRepo;
        _calendarRepo = calendarRepo;
        _appointmentRepo = appointmentRepo;
        _clock = clock;
    }

    public async Task LeaveStoreAsync(int storeId, int professionalId, CancellationToken ct)
    {
        var upcoming = await _appointmentRepo.GetUpcomingByStoreIdAndProfessionalId(
            storeId: storeId,
            professionalId: professionalId,
            ct: ct
        );

        if (upcoming.Any())
            throw new ApplicationLayerValidationException("Cannot leave store with upcoming appointments.");

        var assignments = await _assignmentsRepo.GetByStoreIdAsync(storeId, ct);

        assignments?.UnassignAllForProfessional(professionalId);

        await _calendarRepo.RemoveAsync(storeId, ct);

        var staff = await _staffRepo.GetByStoreId(storeId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff not found.");

        staff.RemoveFromStaff(professionalId, _clock);
    }
}