
using Application.Abstractions.Policies.Appointments;
using Application.Abstractions.Repositories;
using Application.Abstractions.UnitOfWork;
using Application.Abstractions.Validation.Appointments;
using Application.Exceptions;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects.Appointments;
using Domain.ValueObjects.Finance;
using Domain.ValueObjects.Time;

namespace Application.Commands.Appointments;

public sealed class ScheduleHandler
{
    private readonly IScheduleValidator _validator;
    private readonly ISchedulePolicy _policy;
    private readonly IAppointmentRepository _appointmentRepo;
    private readonly IStoreCalendarRepository _storeCalendarRepo;
    private readonly IStaffCalendarRepository _staffCalendarRepo;
    private readonly IClock _clock;
    private readonly IUnitOfWork _uow;

    public ScheduleHandler(
        IScheduleValidator validator,
        ISchedulePolicy policy,
        IAppointmentRepository appointmentRepo,
        IStaffRepository staffRepo,
        IStoreCalendarRepository storeCalendarRepo,
        IStaffCalendarRepository staffCalendarRepo,
        IClock clock,
        IUnitOfWork uow
    )
    {
        _validator = validator;
        _policy = policy;
        _appointmentRepo = appointmentRepo;
        _storeCalendarRepo = storeCalendarRepo;
        _staffCalendarRepo = staffCalendarRepo;
        _clock = clock;
        _uow = uow;
    }

    public async Task<int> Handle(ScheduleCommand command, CancellationToken ct)
    {
        var duration = Duration.FromMinutes(command.Duration);
        var startAt = UtcDateTime.FromUtc(command.StartAt);
        var endAt = startAt.Add(duration.Value);

        var appointment = Appointment.Create(
            userId: command.UserId,
            professionalId: command.ProfessionalId,
            offeringId: command.OfferingId,
            storeId: command.StoreId,
            startAt: startAt,
            duration: duration,
            price: Money.Create(command.Price, command.Currency),
            notes: Notes.From(command.Notes),
            clock: _clock
        );

        var storeCalendar = await _storeCalendarRepo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Store calendar not found.");

        var staffCalendar = await _staffCalendarRepo.GetAsync(command.StoreId, command.ProfessionalId, ct)
            ?? throw new ApplicationLayerNotFoundException("Professional calendar not found.");

        var appointments = await _appointmentRepo.GetByProfessionalIdAsync(command.ProfessionalId, ct);

        _validator.EnsureAvailable(
            storeCalendar,
            staffCalendar,
            appointments,
            startAt,
            endAt
        );

        _policy.EnsureCanCreate(appointment);

        await _appointmentRepo.AddAsync(appointment, ct);

        await _uow.SaveChangesAsync(ct);

        return appointment.Id;
    }
}