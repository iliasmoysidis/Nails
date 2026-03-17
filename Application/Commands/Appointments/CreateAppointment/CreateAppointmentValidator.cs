using Application.Abstractions.Validation;
using Application.Guards;

namespace Application.Commands.Appointments;

public sealed class CreateAppointmentValidator
    : IRequestValidator<CreateAppointmentCommand>
{
    private readonly ValidationGuard _val;
    private readonly CreateAppointmentContext _ctx;

    public CreateAppointmentValidator(ValidationGuard val, CreateAppointmentContext ctx)
    {
        _val = val;
        _ctx = ctx;
    }

    public Task ValidateAsync(CreateAppointmentCommand command, CancellationToken ct)
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