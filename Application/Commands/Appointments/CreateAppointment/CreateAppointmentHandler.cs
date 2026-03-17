
using Application.Abstractions.Repositories;

using Application.Guards;
using Application.Exceptions;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects.Appointments;

namespace Application.Commands.Appointments;

public sealed class CreateAppointmentHandler
{
    private readonly ValidationGuard _val;
    private readonly AuthorizationGuard _auth;
    private readonly IAppointmentRepository _appointmentRepo;
    private readonly IStoreCalendarRepository _storeCalendarRepo;
    private readonly IStaffCalendarRepository _staffCalendarRepo;
    private readonly IStoreCatalogRepository _storeCatalogRepo;
    private readonly IAssignmentsRepository _assignmentsRepo;
    private readonly IOfferingRepository _offeringRepo;
    private readonly IClock _clock;

    public CreateAppointmentHandler(
        ValidationGuard val,
        AuthorizationGuard auth,
        IAppointmentRepository appointmentRepo,
        IStoreCalendarRepository storeCalendarRepo,
        IStaffCalendarRepository staffCalendarRepo,
        IStoreCatalogRepository storeCatalogRepo,
        IAssignmentsRepository assignmentsRepo,
        IOfferingRepository offeringRepo,
        IClock clock
    )
    {
        _val = val;
        _auth = auth;
        _appointmentRepo = appointmentRepo;
        _storeCalendarRepo = storeCalendarRepo;
        _staffCalendarRepo = staffCalendarRepo;
        _storeCatalogRepo = storeCatalogRepo;
        _assignmentsRepo = assignmentsRepo;
        _offeringRepo = offeringRepo;
        _clock = clock;
    }

    public async Task<int> Handle(CreateAppointmentCommand command, CancellationToken ct)
    {
        var storeCatalog = await _storeCatalogRepo.GetByIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Store catalog not found");

        var offering = await _offeringRepo.GetByIdAsync(command.OfferingId, ct)
            ?? throw new ApplicationLayerNotFoundException("Offering not found.");

        var storeCalendar = await _storeCalendarRepo.GetByIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Store calendar not found.");

        var staffCalendar = await _staffCalendarRepo.GetAsync(command.StoreId, command.ProfessionalId, ct)
            ?? throw new ApplicationLayerNotFoundException("Professional calendar not found.");

        var assignments = await _assignmentsRepo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Professional offerings not found.");

        var appointments = await _appointmentRepo.GetByProfessionalIdAsync(command.ProfessionalId, ct);
        var duration = offering.Duration;
        var startAt = command.StartAt;
        var endAt = startAt.Add(duration.Value);

        _val.EnsureAppointmentAvailable(
            storeCalendar,
            staffCalendar,
            appointments,
            startAt,
            endAt
        );
        _val.EnsureStoreOffersService(storeCatalog, command.OfferingId);
        _val.EnsureProfessionalOffersService(
            assignments,
            command.ProfessionalId,
            command.OfferingId
        );

        _auth.EnsureUser();
        _auth.EnsureSelf(command.UserId);

        var appointment = Appointment.Create(
            userId: command.UserId,
            professionalId: command.ProfessionalId,
            offeringId: command.OfferingId,
            storeId: command.StoreId,
            startAt: startAt,
            duration: duration,
            price: offering.Price,
            notes: Notes.From(command.Notes),
            clock: _clock
        );

        await _appointmentRepo.AddAsync(appointment, ct);

        return appointment.Id;
    }
}