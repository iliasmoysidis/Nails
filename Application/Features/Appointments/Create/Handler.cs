
using Application.Abstractions.Repositories;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects.Appointments;
using MediatR;

namespace Application.Features.Appointments.Create;

public sealed class Handler
    : IRequestHandler<Command, int>
{
    private readonly Context _ctx;
    private readonly IAppointmentRepository _repo;

    private readonly IClock _clock;

    public Handler(
        Context ctx,
        IAppointmentRepository repo,
        IClock clock
    )
    {
        _ctx = ctx;
        _repo = repo;
        _clock = clock;
    }

    public async Task<int> Handle(Command command, CancellationToken ct)
    {
        var appointment = Appointment.Create(
            userId: command.UserId,
            professionalId: command.ProfessionalId,
            offeringId: command.OfferingId,
            storeId: command.StoreId,
            startAt: command.StartAt,
            duration: _ctx.Offering.Duration,
            price: _ctx.Offering.Price,
            notes: Notes.From(command.Notes),
            clock: _clock
        );

        await _repo.AddAsync(appointment, ct);

        return appointment.Id;
    }
}