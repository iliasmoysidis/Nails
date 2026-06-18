
using Application.Abstractions.Repositories;
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
        _ctx.User.EnsureActive();

        var appointment = _ctx.AppointmentBooking.Book(
            userId: command.UserId,
            offeringId: command.OfferingId,
            startAt: command.StartAt,
            notes: Notes.From(command.Notes),
            clock: _clock
        );

        _ctx.UserSchedule.Add(appointment);

        await _repo.AddAsync(appointment, ct);

        return appointment.Id;
    }
}
