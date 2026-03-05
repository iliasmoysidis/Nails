using Application.Abstractions.Policies.Appointments;
using Application.Abstractions.Repositories;
using Application.Abstractions.UnitOfWork;
using Application.Exceptions;
using Domain.Interfaces;

namespace Application.Commands.Appointments;

public sealed class MarkNoShowHandler
{
    private readonly IMarkNoShowPolicy _policy;
    private readonly IAppointmentRepository _appointmentRepo;
    private readonly IStaffRepository _staffRepo;
    private readonly IClock _clock;
    private readonly IUnitOfWork _uow;

    public MarkNoShowHandler(
        IMarkNoShowPolicy policy,
        IAppointmentRepository appointmentRepo,
        IStaffRepository staffRepo,
        IClock clock,
        IUnitOfWork uow
    )
    {
        _policy = policy;
        _appointmentRepo = appointmentRepo;
        _staffRepo = staffRepo;
        _clock = clock;
        _uow = uow;
    }

    public async Task Handle(MarkNoShowCommand command, CancellationToken ct)
    {
        var appointment = await _appointmentRepo.GetByIdAsync(command.AppointmentId, ct)
            ?? throw new ApplicationLayerNotFoundException("Appointment not found.");

        var staff = await _staffRepo.GetByStoreId(appointment.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff not found");

        _policy.EnsureCanMarkNoShow(staff);

        appointment.MarkAsNoShow(_clock);

        await _uow.SaveChangesAsync(ct);
    }
}