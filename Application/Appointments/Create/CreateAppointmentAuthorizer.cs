using Application.Common.Abstractions.Authorization;
using Application.Common.Guards;

namespace Application.Appointments.Create;

public sealed class CreateAppointmentAuthorizer
    : IAuthorizer<CreateAppointmentCommand>
{
    private readonly AuthorizationGuard _auth;

    public CreateAppointmentAuthorizer(AuthorizationGuard auth)
    {
        _auth = auth;
    }

    public Task AuthorizeAsync(CreateAppointmentCommand request, CancellationToken ct)
    {
        _auth.EnsureUser();
        _auth.EnsureSelf(request.UserId);

        return Task.CompletedTask;
    }
}