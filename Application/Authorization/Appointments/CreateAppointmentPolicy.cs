using Application.Abstractions.Policies.Appointments;
using Application.Abstractions.Repositories;
using Application.Commands.Appointments;
using Application.Contexts;
using Application.Exceptions;

namespace Application.Authorization.Appointments;

public sealed class CreateAppointmentPolicy : ICreateAppointmentPolicy
{
    private readonly IRequestContext _context;
    private readonly IStaffRepository _repo;

    public CreateAppointmentPolicy(IRequestContext context, IStaffRepository repo)
    {
        _context = context;
        _repo = repo;
    }

    public async Task EnsureCanCreateAsync(CreateAppointmentCommand command, CancellationToken ct)
    {
        if (_context.IsUser)
        {
            EnsureUserCanCreate(command);
            return;
        }

        if (_context.IsProfessional)
        {
            await EnsureProfessionalCanCreate(command, ct);
            return;
        }

        throw new ApplicationLayerForbiddenException("Not allowed to create appointments.");
    }

    private void EnsureUserCanCreate(CreateAppointmentCommand command)
    {
        if (_context.ActorId != command.UserId)
            throw new ApplicationLayerForbiddenException("Users can only book appointments for themselves.");
    }

    private async Task EnsureProfessionalCanCreate(CreateAppointmentCommand command, CancellationToken ct)
    {
        var staff = await _repo.GetByStoreId(command.StoreId, ct);

        if (staff is null)
            throw new ApplicationLayerForbiddenException("Not allowed to create appointments.");

        if (!staff.IsStaff(_context.ActorId))
            throw new ApplicationLayerForbiddenException("Professional does not work for this store.");
    }
}