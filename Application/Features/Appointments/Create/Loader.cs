using Application.Abstractions.Context;
using Application.Abstractions.Repositories;
using Application.Exceptions;

namespace Application.Features.Appointments.Create;

public sealed class Loader
    : IRequestContextLoader<Command, Context>
{
    private readonly IAppointmentRepository _appointmentRepo;
    private readonly IStoreCalendarRepository _storeCalendarRepo;
    private readonly IStaffCalendarRepository _staffCalendarRepo;
    private readonly IStoreCatalogRepository _storeCatalogRepo;
    private readonly IAssignmentsRepository _assignmentsRepo;
    private readonly IOfferingRepository _offeringRepo;

    public Loader(
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
        Command command,
        Context ctx,
        CancellationToken ct)
    {
        var storeCatalog = await _storeCatalogRepo.GetByIdAsync(command.StoreId, ct)
    ?? throw new ApplicationLayerNotFoundException("Store catalog not found");

        var offering = await _offeringRepo.GetByIdAsync(command.OfferingId, ct)
            ?? throw new ApplicationLayerNotFoundException("Offering not found");

        var storeCalendar = await _storeCalendarRepo.GetByIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Store calendar not found");

        var staffCalendar = await _staffCalendarRepo.GetAsync(command.StoreId, command.ProfessionalId, ct)
            ?? throw new ApplicationLayerNotFoundException("Professional calendar not found");

        var assignments = await _assignmentsRepo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Assignments not found");

        var appointments = await _appointmentRepo.GetByProfessionalIdAsync(command.ProfessionalId, ct);

        ctx.StoreCatalog = storeCatalog;
        ctx.Offering = offering;
        ctx.StoreCalendar = storeCalendar;
        ctx.StaffCalendar = staffCalendar;
        ctx.Assignments = assignments;
        ctx.Appointments = appointments;
    }
}