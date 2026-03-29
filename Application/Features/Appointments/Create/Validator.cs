using Application.Abstractions.Validation;
using Application.Guards;

namespace Application.Features.Appointments.Create;

public sealed class Validator
    : IRequestValidator<Command>
{
    private readonly ValidationGuard _val;
    private readonly Context _ctx;

    public Validator(ValidationGuard val, Context ctx)
    {
        _val = val;
        _ctx = ctx;
    }

    public Task ValidateAsync(Command command, CancellationToken ct)
    {
        var duration = _ctx.Offering.Duration;
        var startAt = command.StartAt;
        var endAt = startAt.Add(duration.Value);

        _val.EnsureAppointmentAvailable(
            _ctx.StoreCalendar,
            _ctx.StaffCalendar,
            _ctx.Appointments,
            startAt,
            endAt
        );
        _val.EnsureStoreOffersService(_ctx.StoreCatalog, command.OfferingId);
        _val.EnsureProfessionalOffersService(
            _ctx.Assignments,
            command.ProfessionalId,
            command.OfferingId
        );

        return Task.CompletedTask;
    }
}