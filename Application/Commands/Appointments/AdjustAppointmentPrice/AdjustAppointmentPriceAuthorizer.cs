using Application.Abstractions.Authorization;
using Application.Guards;

namespace Application.Commands.Appointments;

public sealed class AdjustAppointmentPriceAuthorizer
    : IAuthorizer<AdjustAppointmentPriceCommand>
{
    private readonly AuthorizationGuard _auth;
    private readonly AdjustAppointmentPriceContext _ctx;

    public AdjustAppointmentPriceAuthorizer(
        AuthorizationGuard auth,
        AdjustAppointmentPriceContext ctx
    )
    {
        _auth = auth;
        _ctx = ctx;
    }

    public Task AuthorizeAsync(AdjustAppointmentPriceCommand request, CancellationToken ct)
    {
        _auth.EnsureOwner(_ctx.Staff);

        return Task.CompletedTask;
    }
}