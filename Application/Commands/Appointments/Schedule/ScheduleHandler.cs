
using Application.Abstractions.Repositories;
using Application.Abstractions.UnitOfWork;
using Application.Guards;
using Application.Exceptions;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects.Appointments;

namespace Application.Commands.Appointments;

public sealed class ScheduleHandler
{
    private readonly ValidationGuard _val;
    private readonly AuthorizationGuard _auth;
    private readonly IAppointmentRepository _appointmentRepo;
    private readonly IStoreCalendarRepository _storeCalendarRepo;
    private readonly IStaffCalendarRepository _staffCalendarRepo;
    private readonly IStoreCatalogRepository _storeCatalogRepo;
    private readonly IProfessionalOfferingsRepository _professionalOfferingRepo;
    private readonly IClock _clock;
    private readonly IUnitOfWork _uow;

    public ScheduleHandler(
        ValidationGuard val,
        AuthorizationGuard auth,
        IAppointmentRepository appointmentRepo,
        IStoreCalendarRepository storeCalendarRepo,
        IStaffCalendarRepository staffCalendarRepo,
        IStoreCatalogRepository storeCatalogRepo,
        IProfessionalOfferingsRepository professionalOfferingsRepo,
        IClock clock,
        IUnitOfWork uow
    )
    {
        _val = val;
        _auth = auth;
        _appointmentRepo = appointmentRepo;
        _storeCalendarRepo = storeCalendarRepo;
        _staffCalendarRepo = staffCalendarRepo;
        _storeCatalogRepo = storeCatalogRepo;
        _professionalOfferingRepo = professionalOfferingsRepo;
        _clock = clock;
        _uow = uow;
    }

    public async Task<int> Handle(ScheduleCommand command, CancellationToken ct)
    {
        var storeCatalog = await _storeCatalogRepo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Store catalog not found");

        var offering = storeCatalog.GetOffering(command.OfferingId)
            ?? throw new ApplicationLayerNotFoundException("Offering not found.");

        var storeCalendar = await _storeCalendarRepo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Store calendar not found.");

        var staffCalendar = await _staffCalendarRepo.GetAsync(command.StoreId, command.ProfessionalId, ct)
            ?? throw new ApplicationLayerNotFoundException("Professional calendar not found.");

        var assignments = await _professionalOfferingRepo.GetByStoreIdAsync(command.StoreId, ct)
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
        _val.EnsureProfessionalOffering(
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

        await _uow.SaveChangesAsync(ct);

        return appointment.Id;
    }
}