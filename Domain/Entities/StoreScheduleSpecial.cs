using Domain.Common;
using Domain.Exceptions;

namespace Domain.Entities;

public class StoreScheduleSpecial : BaseEntity
{
    public int Id { get; private set; }
    public int StoreId { get; private set; }
    public DateTime Date { get; private set; }
    public TimeSpan? OpenTime { get; private set; }
    public TimeSpan? CloseTime { get; private set; }
    public string? Reason { get; private set; }

    private StoreScheduleSpecial() { }

    public static StoreScheduleSpecial Create(int storeId, DateTime date, TimeSpan? openTime = null, TimeSpan? closeTime = null, string? reason = null)
    {
        ValidateScheduleInfo(date, openTime, closeTime, reason);

        return new StoreScheduleSpecial
        {
            StoreId = storeId,
            Date = date.Date,
            OpenTime = openTime,
            CloseTime = closeTime,
            Reason = reason
        };
    }

    private static void ValidateScheduleInfo(DateTime date, TimeSpan? openTime = null, TimeSpan? closeTime = null, string? reason = null)
    {
        if ((openTime.HasValue && !closeTime.HasValue) || (!openTime.HasValue && closeTime.HasValue))
        {
            throw new DomainException("Both open and close times must be provided.");
        }

        if (openTime.HasValue && closeTime.HasValue && openTime >= closeTime)
        {
            throw new DomainException("Opening time must be earlier than closing time.");
        }

        if (date.Date < DateTime.UtcNow.Date)
        {
            throw new DomainException("Cannot create an exception for a past date");
        }

        if (reason?.Length > 500)
        {
            throw new DomainException("Reason cannot be longer than 500 characters.");
        }
    }

    public bool IsFullDayClosed => !OpenTime.HasValue && !CloseTime.HasValue;
}
