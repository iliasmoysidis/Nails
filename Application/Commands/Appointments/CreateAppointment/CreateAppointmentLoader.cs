using Application.Abstractions.Repositories;
using Application.Exceptions;

namespace Application.Commands.Appointments;

public sealed class CreateAppointmentLoader
    : IRequestContextLoader<CreateAppointmentCommand, CreateAppointmentContext>
{
    private readonly IAppointmentRepository _appointmentRepo;
    private readonly IStoreCalendarRepository _storeCalendarRepo;
    private readonly IStaffCalendarRepository _staffCalendarRepo;
    private readonly IStoreCatalogRepository _storeCatalogRepo;
    private readonly IAssignmentsRepository _assignmentsRepo;
    private readonly IOfferingRepository _offeringRepo;

    public CreateAppointmentLoader(
    IStoreCatalogRepository storeCatalogRepo,
    IStaffCalendarRepository staffCalendarRepo,
    IStoreCalendarRepository storeCalendarRepo,
    IAssignmentsRepository assignmentsRepo,
    IAppointmentRepository appointmentRepo,
    IOfferingRepository offeringRepo)
    {
        _storeCatalogRepo = storeCatalogRepo;
        _staffCalendarRepo = staffCalendarRepo;
        _storeCalendarRepo = storeCalendarRepo;
        _assignmentsRepo = assignmentsRepo;
        _appointmentRepo = appointmentRepo;
        _offeringRepo = offeringRepo;
    }

    public async Task PopulateAsync(
        CreateAppointmentCommand command,
        CreateAppointmentContext ctx,
        CancellationToken ct)
    {
        ctx.StoreCatalog = await _storeCatalogRepo.GetByIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Store catalog not found");

        ctx.Offering = await _offeringRepo.GetByIdAsync(command.OfferingId, ct)
            ?? throw new ApplicationLayerNotFoundException("Offering not found");

        ctx.StoreCalendar = await _storeCalendarRepo.GetByIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Store calendar not found");

        ctx.StaffCalendar = await _staffCalendarRepo.GetAsync(command.StoreId, command.ProfessionalId, ct)
            ?? throw new ApplicationLayerNotFoundException("Professional calendar not found");

        ctx.Assignments = await _assignmentsRepo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Assignments not found");

        ctx.Appointments = await _appointmentRepo.GetByProfessionalIdAsync(command.ProfessionalId, ct);
    }
}