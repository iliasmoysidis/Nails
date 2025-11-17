using Domain.Common;
using Domain.Exceptions;

namespace Domain.Entities;

public class StoreException : BaseEntity
{
    public int Id { get; private set; }
    public int StoreId { get; private set; }
    public DateTime Date { get; private set; }
    public TimeSpan? OpenTime { get; private set; }
    public TimeSpan? CloseTime { get; private set; }
    public string? Reason { get; private set; }
    public bool IsClosed { get; private set; }

    public Store Store { get; private set; } = null!;

    private StoreException()
    {
        IsClosed = true;
    }

    public static StoreException Create(int storeId, DateTime date, TimeSpan? openTime = null, TimeSpan? closeTime = null, string? reason = null, bool isClosed = true)
    {
        if (openTime.HasValue && closeTime.HasValue && openTime >= closeTime)
        {
            throw new DomainException("Opening time must  be earlier than closing time");
        }

        return new StoreException
        {
            StoreId = storeId,
            Date = date,
            OpenTime = openTime,
            CloseTime = closeTime,
            Reason = reason,
            IsClosed = isClosed
        };
    }
}
