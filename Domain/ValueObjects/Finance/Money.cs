using System.Runtime;
using Domain.Exceptions;

namespace Domain.ValueObjects.Finance;

public sealed record Money
{
    public decimal Amount { get; }
    public string Currency { get; }

    private Money(decimal amount, string currency)
    {
        Amount = amount;
        Currency = currency;
    }

    public static Money Create(decimal amount, string currency)
    {
        if (amount < 0)
            throw new DomainException("Money amount cannot be negative.");

        if (string.IsNullOrWhiteSpace(currency))
            throw new DomainException("Currency is required.");

        return new Money(decimal.Round(amount, 2, MidpointRounding.AwayFromZero), currency.ToUpperInvariant());
    }

    public static Money EUR(decimal amount) => Create(amount, "EUR");
    public static Money USD(decimal amount) => Create(amount, "USD");

    public static Money operator +(Money a, Money b)
    {
        EnsureNotNull(a, b);
        a.EnsureSameCurrency(b);
        return Create(a.Amount + b.Amount, a.Currency);
    }

    public static Money operator -(Money a, Money b)
    {
        EnsureNotNull(a, b);
        a.EnsureSameCurrency(b);

        if (a < b)
            throw new DomainException("Insufficient amount.");

        return Create(a.Amount - b.Amount, a.Currency);
    }

    public static bool operator <(Money a, Money b)
    {
        EnsureNotNull(a, b);
        a.EnsureSameCurrency(b);
        return a.Amount < b.Amount;
    }

    public static bool operator >(Money a, Money b)
    {
        EnsureNotNull(a, b);
        a.EnsureSameCurrency(b);
        return a.Amount > b.Amount;
    }

    public static bool operator <=(Money a, Money b)
    {
        EnsureNotNull(a, b);
        a.EnsureSameCurrency(b);
        return a.Amount <= b.Amount;
    }

    public static bool operator >=(Money a, Money b)
    {
        EnsureNotNull(a, b);
        a.EnsureSameCurrency(b);
        return a.Amount >= b.Amount;
    }

    private void EnsureSameCurrency(Money other)
    {
        if (Currency != other.Currency)
            throw new DomainException("Cannot operate on different currencies.");
    }

    private static void EnsureNotNull(Money a, Money b)
    {
        if (a is null || b is null)
            throw new ArgumentNullException();
    }

    public override string ToString() => $"{Amount} {Currency}";
}