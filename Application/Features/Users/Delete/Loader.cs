using Application.Abstractions.Context;
using Application.Abstractions.Repositories;
using Application.Exceptions;
using Domain.Services;


namespace Application.Features.Users.Delete;

public sealed class Loader : IRequestContextLoader<Command, Context>
{
    private readonly IUserRepository _userRepo;
    private readonly IAppointmentRepository _appointmentRepo;

    public Loader(IUserRepository userRepo, IAppointmentRepository appointmentRepo)
    {
        _userRepo = userRepo;
        _appointmentRepo = appointmentRepo;
    }

    public async Task PopulateAsync(Command command, Context ctx, CancellationToken ct)
    {
        var user = await _userRepo.GetByIdAsync(command.UserId, ct)
            ?? throw new ApplicationLayerNotFoundException("User not found.");

        var appointments = await _appointmentRepo.GetUpcomingByUserIdAsync(command.UserId, ct);

        ctx.UserDeletion = new UserDeletion(user, appointments);
    }
}
