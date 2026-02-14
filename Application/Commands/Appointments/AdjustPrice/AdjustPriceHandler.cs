using Application.Abstractions.Policies.Appointments;
using Application.Abstractions.Repositories;
using Application.Abstractions.UnitOfWork;
using Application.Exceptions;
using Domain.Interfaces;
using Domain.ValueObjects.Finance;

namespace Application.Commands.Appointments;

public sealed class AdjustPriceHandler
{
    private readonly IAdjustAppointmentPricePolicy _policy;
    private readonly IAppointmentRepository _repo;
    private readonly IClock _clock;
    private readonly IUnitOfWork _uow;

    public AdjustPriceHandler(
        IAdjustAppointmentPricePolicy policy,
        IAppointmentRepository repo,
        IClock clock,
        IUnitOfWork uow
    )
    {
        _policy = policy;
        _repo = repo;
        _clock = clock;
        _uow = uow;
    }

    public async Task Handle(AdjustPriceCommand command, CancellationToken ct)
    {
        await _policy.EnsureCanAdjustPriceAsync(command, ct);

        var appointment = await _repo.GetByIdAsync(command.AppointmentId, ct)
            ?? throw new ApplicationLayerNotFoundException("Appointment not found.");

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