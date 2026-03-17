using Application.Abstractions.Repositories;
using Application.Guards;
using Application.Exceptions;
using Domain.Interfaces;
using Domain.ValueObjects.Finance;

namespace Application.Commands.Appointments;

public sealed class AdjustAppointmentPriceHandler
{
    private readonly AuthorizationGuard _auth;
    private readonly IAppointmentRepository _appointmentRepo;
    private readonly IStaffRepository _staffRepo;
    private readonly IClock _clock;

    public AdjustAppointmentPriceHandler(
        AuthorizationGuard auth,
        IAppointmentRepository appointmentRepo,
        IStaffRepository staffRepo,
        IClock clock
    )
    {
        _auth = auth;
        _appointmentRepo = appointmentRepo;
        _staffRepo = staffRepo;
        _clock = clock;
    }

    public async Task Handle(AdjustAppointmentPriceCommand command, CancellationToken ct)
    {
        var appointment = await _appointmentRepo.GetByIdAsync(command.AppointmentId, ct)
            ?? throw new ApplicationLayerNotFoundException("Appointment not found.");

        var staff = await _staffRepo.GetByStoreIdAsync(appointment.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff not found.");

        _auth.EnsureOwner(staff);

        appointment.AdjustPrice(
            newPrice: Money.Create(
                amount: command.Money.Amount,
                currency: command.Money.Currency
                ),
            reason: command.Reason,
            clock: _clock
        );
    }
}