
using Application.Appointments.Common.Repositories;
using Domain.UserSchedules;
using Domain.Appointments.Services;
using Domain.Appointments.ValueObjects;
using Domain.Users;
using Domain.Common;
using MediatR;

namespace Application.Appointments.Create;

public sealed class CreateAppointmentHandler
    : IRequestHandler<CreateAppointmentCommand, int>
{
    private readonly CreateAppointmentContext _ctx;
    private readonly IAppointmentRepository _repo;
    private readonly IClock _clock;

    public CreateAppointmentHandler(
        CreateAppointmentContext ctx,
        IAppointmentRepository repo,
        IClock clock
    )
    {
        _ctx = ctx;
        _repo = repo;
        _clock = clock;
    }

    public async Task<int> Handle(CreateAppointmentCommand command, CancellationToken ct)
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
