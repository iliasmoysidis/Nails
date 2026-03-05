using Application.Abstractions.Policies.Appointments;
using Application.Abstractions.Repositories;
using Application.Abstractions.UnitOfWork;
using Application.Exceptions;
using Domain.Interfaces;
using Domain.ValueObjects.Finance;

namespace Application.Commands.Appointments;

public sealed class AdjustPriceHandler
{
    private readonly IAdjustPricePolicy _policy;
    private readonly IAppointmentRepository _appointmentRepo;
    private readonly IStaffRepository _staffRepo;
    private readonly IClock _clock;
    private readonly IUnitOfWork _uow;

    public AdjustPriceHandler(
        IAdjustPricePolicy policy,
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

    public async Task Handle(AdjustPriceCommand command, CancellationToken ct)
    {
        var appointment = await _appointmentRepo.GetByIdAsync(command.AppointmentId, ct)
            ?? throw new ApplicationLayerNotFoundException("Appointment not found.");

        var staff = await _staffRepo.GetByStoreId(appointment.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff not found.");

        _policy.EnsureCanAdjustPrice(staff);

        appointment.AdjustPrice(
            newPrice: Money.Create(
                amount: command.Money.Amount,
                currency: command.Money.Currency
                ),
            reason: command.Reason,
            clock: _clock
        );

        await _uow.SaveChangesAsync(ct);
    }
}