using Application.Abstractions.Policies.Appointments;
using Application.Abstractions.Repositories;
using Application.Abstractions.UnitOfWork;
using Application.Contexts;
using Application.Exceptions;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Commands.Appointments;

public sealed class CancelHandler
{
    private readonly IRequestContext _context;
    private readonly ICancelPolicy _policy;
    private readonly IAppointmentRepository _appointmentRepo;
    private readonly IStaffRepository _staffRepo;
    private readonly IClock _clock;
    private readonly IUnitOfWork _uow;

    public CancelHandler(
        IRequestContext context,
        ICancelPolicy policy,
        IAppointmentRepository appointmentRepo,
        IStaffRepository staffRepo,
        IClock clock,
        IUnitOfWork uow
    )
    {
        _context = context;
        _policy = policy;
        _appointmentRepo = appointmentRepo;
        _staffRepo = staffRepo;
        _clock = clock;
        _uow = uow;
    }

    public async Task Handle(CancelCommand command, CancellationToken ct)
    {
        var appointment = await _appointmentRepo.GetByIdAsync(command.AppointmentId, ct)
            ?? throw new ApplicationLayerNotFoundException("Appointment not found.");

        Staff? staff = null;

        if (_context.IsProfessional)
        {
            staff = await _staffRepo.GetByStoreId(appointment.StoreId, ct)
                ?? throw new ApplicationLayerNotFoundException("Staff not found.");
        }

        _policy.EnsureCanCancel(appointment, staff);

        appointment.Cancel(_clock, command.Reason);

        await _uow.SaveChangesAsync(ct);
    }
}