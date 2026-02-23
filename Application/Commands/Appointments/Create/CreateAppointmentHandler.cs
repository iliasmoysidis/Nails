
using Application.Abstractions.Policies.Appointments;
using Application.Abstractions.Repositories;
using Application.Abstractions.UnitOfWork;
using Application.Abstractions.Validation.Appointments;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects.Appointments;
using Domain.ValueObjects.Finance;
using Domain.ValueObjects.Time;

namespace Application.Commands.Appointments;

public sealed class CreateAppointmentHandler
{
    private readonly IScheduleValidator _validator;
    private readonly ICreateAppointmentPolicy _policy;
    private readonly IAppointmentRepository _repo;
    private readonly IClock _clock;
    private readonly IUnitOfWork _uow;

    public CreateAppointmentHandler(
        IScheduleValidator validator,
        ICreateAppointmentPolicy policy,
        IAppointmentRepository repo,
        IClock clock,
        IUnitOfWork uow
    )
    {
        _validator = validator;
        _policy = policy;
        _repo = repo;
        _clock = clock;
        _uow = uow;
    }

    public async Task<int> Handle(CreateAppointmentCommand command, CancellationToken ct)
    {
        await _validator.EnsureAvailableAsync(command, ct);

        await _policy.EnsureCanCreateAsync(command, ct);

        var startAt = UtcDateTime.FromUtc(command.StartAt);
        var duration = Duration.FromMinutes(command.Duration);
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

        await _repo.AddAsync(appointment, ct);
        await _uow.SaveChangesAsync(ct);

        return appointment.Id;
    }
}