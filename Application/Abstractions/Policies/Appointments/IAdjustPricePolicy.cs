using Application.Commands.Appointments;

namespace Application.Abstractions.Policies.Appointments;

public interface IAdjustPricePolicy
{
    Task EnsureCanAdjustPriceAsync(AdjustPriceCommand command, CancellationToken ct);
}