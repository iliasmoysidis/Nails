using Application.Commands.Appointments;

namespace Application.Abstractions.Policies.Appointments;

public interface IAdjustAppointmentPricePolicy
{
    Task EnsureCanAdjustPriceAsync(AdjustPriceCommand command, CancellationToken ct);
}