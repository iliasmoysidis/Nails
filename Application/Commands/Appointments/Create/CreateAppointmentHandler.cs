
using Application.Abstractions.Policies.Appointments;
using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Abstractions.UnitOfWork;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects.Appointments;
using Domain.ValueObjects.Finance;
using Domain.ValueObjects.Time;

namespace Application.Commands.Appointments;

public sealed class CreateAppointmentHandler
{
    private readonly IAppointmentAvailabilityService _availability;
    private readonly ICreateAppointmentPolicy _policy;
    private readonly IAppointmentRepository _repo;
    private readonly IClock _clock;
    private readonly IUnitOfWork _uow;

    public CreateAppointmentHandler(
        IAppointmentAvailabilityService availability,
        ICreateAppointmentPolicy policy,
        IAppointmentRepository repo,
        IClock clock,
        IUnitOfWork uow
    )
    {
        _availability = availability;
        _policy = policy;
        _repo = repo;
        _clock = clock;
        _uow = uow;
    }

    public async Task<int> Handle(CreateAppointmentCommand command, CancellationToken ct)
    {
        var startAt = UtcDateTime.FromUtc(command.StartAt);
        var duration = Duration.FromMinutes(command.Duration);
        var endAt = startAt.Add(duration.Value);

        await _policy.EnsureCanCreateAsync(command, ct);
        await _availability.EnsureAvailableAsync(
            storeId: command.StoreId,
            professionalId: command.ProfessionalId,
            startAt: startAt,
            endAt: endAt,
            ignoreAppointmentId: null,
            ct: ct);

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

        await _repo.AddAsync(appointment, ct);
        await _uow.SaveChangesAsync(ct);

        return appointment.Id;
    }
}