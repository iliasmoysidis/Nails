
using Application.Abstractions.Policies;
using Application.Abstractions.Repositories;
using Application.Abstractions.UnitOfWork;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects.Appointments;
using Domain.ValueObjects.Finance;
using Domain.ValueObjects.Time;

namespace Application.Commands.Appointments;

public sealed class CreateAppointmentHandler
{
    private readonly ICreateAppointmentPolicy _policy;
    private readonly IAppointmentRepository _repo;
    private readonly IClock _clock;
    private readonly IUnitOfWork _uow;

    public CreateAppointmentHandler(
        ICreateAppointmentPolicy policy,
        IAppointmentRepository repo,
        IClock clock,
        IUnitOfWork uow
    )
    {
        _policy = policy;
        _repo = repo;
        _clock = clock;
        _uow = uow;
    }

    public async Task<int> Handle(CreateAppointmentCommand command, CancellationToken ct)
    {
        await _policy.EnsureCanCreateAsync(command, ct);

        var appointment = Appointment.Create(
            userId: command.UserId,
            professionalId: command.ProfessionalId,
            offeringId: command.OfferingId,
            storeId: command.StoreId,
            startAt: UtcDateTime.FromUtc(command.StartAt),
            duration: Duration.FromMinutes(command.Duration),
            price: Money.Create(command.Price, command.Currency),
            notes: Notes.From(command.Notes),
            clock: _clock
        );

        await _repo.AddAsync(appointment, ct);
        await _uow.SaveChangesAsync(ct);

        return appointment.Id;
    }
}