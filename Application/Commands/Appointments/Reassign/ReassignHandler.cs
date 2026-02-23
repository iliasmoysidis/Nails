using Application.Abstractions.Policies.Appointments;
using Application.Abstractions.Repositories;
using Application.Abstractions.UnitOfWork;
using Application.Abstractions.Validation.Appointments;
using Application.Exceptions;
using Domain.Interfaces;

namespace Application.Commands.Appointments;

public sealed class ReassignHandler
{
    private readonly IReassignValidator _validator;
    private readonly IReassignPolicy _policy;
    private readonly IAppointmentRepository _repo;
    private readonly IClock _clock;
    private readonly IUnitOfWork _uow;

    public ReassignHandler(
        IReassignValidator validator,
        IReassignPolicy policy,
        IAppointmentRepository repo,
        IClock clock,
        IUnitOfWork uow
    )
    {
        _validator = validator;
        _policy = policy;
        _repo = repo;
        _clock = clock;
        _uow = uow;
    }

    public async Task Handle(ReassignCommand command, CancellationToken ct)
    {
        await _validator.EnsureAvailableAsync(command, ct);

        await _policy.EnsureCanReassignAsync(command, ct);

        var appointment = await _repo.GetByIdAsync(command.AppointmentId, ct)
            ?? throw new ApplicationLayerNotFoundException("Appointment not found.");

        appointment.Reassign(command.ProfessionalId, _clock);

        await _uow.SaveChangesAsync(ct);
    }
}