using Domain.Common;
using Domain.Exceptions;

namespace Domain.Entities;

public class Store : HistoricEntity
{
    public int Id { get; private set; }
    public string Name { get; private set; } = null!;
    public string Address { get; private set; } = null!;
    public string TaxIdNumber { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string Phone { get; private set; } = null!;

    private readonly List<StoreOwner> _owners = new();
    public IReadOnlyCollection<StoreOwner> Owners => _owners.AsReadOnly();

    private readonly List<StoreProfessional> _staff = new();
    public IReadOnlyCollection<StoreProfessional> Staff => _staff.AsReadOnly();

    private readonly List<StoreProfessionalSchedule> _staffSchedules = new();
    public IReadOnlyCollection<StoreProfessionalSchedule> StaffSchedules => _staffSchedules.AsReadOnly();

    private readonly List<StoreProfessionalException> _professionalExceptions = new();
    public IReadOnlyCollection<StoreProfessionalException> ProfessionalExceptions => _professionalExceptions.AsReadOnly();

    private readonly List<Service> _services = new();
    public IReadOnlyCollection<Service> Services => _services.AsReadOnly();

    private readonly List<ProfessionalService> _professionalServices = new();
    public IReadOnlyCollection<ProfessionalService> ProfessionalServices => _professionalServices.AsReadOnly();

    private readonly List<StoreSchedule> _storeSchedules = new();
    public IReadOnlyCollection<StoreSchedule> StoreSchedules => _storeSchedules.AsReadOnly();

    private readonly List<StoreException> _exceptions = new();
    public IReadOnlyCollection<StoreException> Exceptions => _exceptions.AsReadOnly();

    private Store() { }

    public bool IsOwner(int professionalId)
    {
        return _owners.Any(o => o.ProfessionalId == professionalId);
    }

    public bool IsStaff(int professionalId)
    {
        return _staff.Any(s => s.ProfessionalId == professionalId);
    }

    public bool IsProvided(int professionalId, int serviceId)
    {
        return _professionalServices.Any(ps => ps.ProfessionalId == professionalId && ps.ServiceId == serviceId);
    }

    public static Store Create(string name, string address, string taxIdNumber, string email, string phone)
    {
        ValidateStoreInfo(name, address, taxIdNumber, email, phone);
        return new Store
        {
            Name = name,
            Address = address,
            TaxIdNumber = taxIdNumber,
            Email = email,
            Phone = phone
        };
    }

    public StoreOwner AddOwner(int ownerId, int prospectiveOwnerId)
    {
        if (IsDeleted)
        {
            throw new DomainException("Cannot add owner to an inactive store.");
        }

        if (!IsOwner(ownerId))
        {
            throw new DomainException("Only an owner can add an owner.");
        }

        if (IsOwner(prospectiveOwnerId))
        {
            throw new DomainException("This professional is already an owner of this store.");
        }

        var owner = StoreOwner.Create(Id, prospectiveOwnerId);
        _owners.Add(owner);
        MarkAsUpdated();

        return owner;
    }

    public void RemoveOwner(int ownerId, int oldOwnerId)
    {
        if (IsDeleted)
        {
            throw new DomainException("Cannot remove owner from an inactive store.");
        }

        if (!IsOwner(ownerId))
        {
            throw new DomainException("Only an owner can remove an owner.");
        }

        var oldOwner = _owners.FirstOrDefault(s => s.ProfessionalId == oldOwnerId);

        if (oldOwner == null)
        {
            throw new DomainException("Professional is not an owner of this store.");
        }

        if (_owners.Count() == 1)
        {
            throw new DomainException("Cannot remove last owner.");
        }

        _owners.Remove(oldOwner);
        MarkAsUpdated();
    }

    public StoreProfessional AddStaff(int ownerId, int professionalId)
    {
        if (IsDeleted)
        {
            throw new DomainException("Cannot add staff to an inactive store.");
        }

        if (!IsOwner(ownerId))
        {
            throw new DomainException("Only an owner can add staff.");
        }

        if (IsStaff(professionalId))
        {
            throw new DomainException("Professional already working for the store.");
        }

        var employee = StoreProfessional.Create(Id, professionalId);
        _staff.Add(employee);
        MarkAsUpdated();

        return employee;
    }

    public void RemoveStaff(int ownerId, int professionalId)
    {
        if (IsDeleted)
        {
            throw new DomainException("Cannot remove staff from an inactive store.");
        }

        if (!IsOwner(ownerId))
        {
            throw new DomainException("Only an owner can remove staff.");
        }

        var employee = _staff.FirstOrDefault(s => s.ProfessionalId == professionalId);

        if (employee == null)
        {
            throw new DomainException("Professional does not work for the store.");
        }

        _staff.Remove(employee);
        MarkAsUpdated();
    }

    public Service AddService(int ownerId, string name, decimal price, TimeSpan duration)
    {
        if (IsDeleted)
        {
            throw new DomainException("Cannot add service to an inactive store.");
        }

        if (!IsOwner(ownerId))
        {
            throw new DomainException("Only an owner can add a service.");
        }

        var service = Service.Create(this, name, price, duration);
        _services.Add(service);
        MarkAsUpdated();

        return service;
    }

    public void RemoveService(int ownerId, int serviceId)
    {
        if (IsDeleted)
        {
            throw new DomainException("Cannot remove service from an inactive store.");
        }

        if (!IsOwner(ownerId))
        {
            throw new DomainException("Only an owner can remove a service.");
        }

        var service = _services.FirstOrDefault(s => s.Id == serviceId && s.StoreId == Id);

        if (service == null)
        {
            throw new DomainException("Cannot remove a service that is not offered by the store.");
        }

        service.Deactivate();
        MarkAsUpdated();
    }

    public ProfessionalService AssignStaffToService(int ownerId, int professionalId, int serviceId)
    {
        if (IsDeleted)
        {
            throw new DomainException("Cannot assign services in an inactive store.");
        }

        if (!IsOwner(ownerId))
        {
            throw new DomainException("Only an owner can assign a staff to a service.");
        }

        if (!IsStaff(professionalId))
        {
            throw new DomainException("Cannot assign services to a professional who does not work at the store.");
        }

        if (IsProvided(professionalId, serviceId))
        {
            throw new DomainException("Service is already offered by this professional");
        }

        var service = _services.FirstOrDefault(s => s.Id == serviceId && s.StoreId == Id);

        if (service == null)
        {
            throw new DomainException("Service is not offered by the store.");
        }

        var professionalService = ProfessionalService.Create(professionalId, serviceId);
        _professionalServices.Add(professionalService);
        MarkAsUpdated();

        return professionalService;
    }

    public void UnassignStaffFromService(int ownerId, int professionalId, int serviceId)
    {
        if (IsDeleted)
        {
            throw new DomainException("Cannot unassign services in an inactive store.");
        }

        if (!IsOwner(ownerId))
        {
            throw new DomainException("Only an owner can unassign a staff from a service.");
        }

        if (!IsStaff(professionalId))
        {
            throw new DomainException("Cannot unassign services from professionals who do not work at the store.");
        }

        var service = _services.FirstOrDefault(s => s.Id == serviceId && s.StoreId == Id);

        if (service == null)
        {
            throw new DomainException("Service is not offered by the store.");
        }

        var professionalService = _professionalServices.FirstOrDefault(
            ps => ps.ProfessionalId == professionalId &&
            ps.ServiceId == serviceId);

        if (professionalService == null)
        {
            throw new DomainException("Service is not offered by the professional.");
        }

        _professionalServices.Remove(professionalService);
        MarkAsUpdated();
    }

    public StoreSchedule AddStoreSchedule(int ownerId, DayOfWeek day, TimeSpan? openTime = null, TimeSpan? closeTime = null)
    {
        if (IsDeleted)
        {
            throw new DomainException("Cannot add operating hours in an inactive store.");
        }

        if (!IsOwner(ownerId))
        {
            throw new DomainException("Only an owner can add operating hours.");
        }

        var storeSchedule = StoreSchedule.Create(Id, day, openTime, closeTime);

        if (storeSchedule.IsFullDayClosed)
        {
            bool isPartialOpen = _storeSchedules.Any(e => e.Day == day && !e.IsFullDayClosed);

            if (isPartialOpen)
            {
                throw new DomainException("Cannot create a full-day closure when partial schedule exists for the day.");
            }
        }
        else
        {
            bool isFullClosedDay = _storeSchedules.Any(e => e.Day == day && e.IsFullDayClosed);

            if (isFullClosedDay)
            {
                throw new DomainException("Cannot create a partial block on a fully closed day.");
            }

            bool isOverlapping = _storeSchedules.Any(
                e => e.Day == day &&
                e.OpenTime.HasValue &&
                e.CloseTime.HasValue &&
                e.OpenTime.Value < closeTime &&
                e.CloseTime.Value > openTime
                );

            if (isOverlapping)
            {
                throw new DomainException("Operating hours overlap with an existing schedule for this day.");
            }
        }

        _storeSchedules.Add(storeSchedule);
        MarkAsUpdated();

        return storeSchedule;
    }

    public void RemoveStoreSchedule(int ownerId, int storeScheduleId)
    {
        if (IsDeleted)
        {
            throw new DomainException("Cannot remove operating hours from an inactive store.");
        }

        if (!IsOwner(ownerId))
        {
            throw new DomainException("Only an owner can remove operating hours.");
        }

        var hours = _storeSchedules.FirstOrDefault(h => h.Id == storeScheduleId && h.StoreId == Id);

        if (hours == null)
        {
            throw new DomainException("Operating hours not found.");
        }

        _storeSchedules.Remove(hours);
        MarkAsUpdated();
    }

    public StoreException AddStoreException(int ownerId, DateTime date, TimeSpan? openTime = null, TimeSpan? closeTime = null, string? reason = null)
    {
        if (IsDeleted)
        {
            throw new DomainException("Cannot add exceptions to an inactive store.");
        }

        if (!IsOwner(ownerId))
        {
            throw new DomainException("Only an owner can add exceptions.");
        }

        var exception = StoreException.Create(Id, date, openTime, closeTime, reason);

        if (exception.IsFullDayClosed)
        {
            bool isPartialOpen = _exceptions.Any(e => e.Date.Date == date.Date && !e.IsFullDayClosed);

            if (isPartialOpen)
            {
                throw new DomainException("Cannot create a full-day closure when partial exceptions exist for the day");
            }
        }
        else
        {
            bool isFullClosedDay = _exceptions.Any(e => e.Date.Date == date.Date && e.IsFullDayClosed);

            if (isFullClosedDay)
            {
                throw new DomainException("Cannot create a partial block on a fully closed day.");
            }

            bool isOverlapping = _exceptions.Any(
                e => e.Date.Date == date.Date &&
                e.OpenTime.HasValue &&
                e.CloseTime.HasValue &&
                e.OpenTime.Value < closeTime &&
                e.CloseTime.Value > openTime
                );

            if (isOverlapping)
            {
                throw new DomainException("Exception overlaps with an existing partial block for this day.");
            }
        }

        _exceptions.Add(exception);
        MarkAsUpdated();

        return exception;
    }

    public void RemoveStoreException(int ownerId, int exceptionId)
    {
        if (IsDeleted)
        {
            throw new DomainException("Cannot remove exceptions from an inactive store.");
        }

        if (!IsOwner(ownerId))
        {
            throw new DomainException("Only an owner can remove exceptions.");
        }

        var exception = _exceptions.FirstOrDefault(e => e.Id == exceptionId && e.StoreId == Id);

        if (exception == null)
        {
            throw new DomainException("Exception not found.");
        }

        _exceptions.Remove(exception);
        MarkAsUpdated();
    }

    public StoreProfessionalSchedule AddStaffSchedule(int ownerId, int professionalId, DayOfWeek day, TimeSpan? startTime = null, TimeSpan? endTime = null)
    {
        if (IsDeleted)
        {
            throw new DomainException("Cannot add schedules to an inactive store.");
        }

        if (!IsOwner(ownerId))
        {
            throw new DomainException("Only an owner can add staff schedules.");
        }

        if (!IsStaff(professionalId))
        {
            throw new DomainException("Cannot set schedule for a professional who does not work at this store.");
        }

        var schedule = StoreProfessionalSchedule.Create(Id, professionalId, day, startTime, endTime);

        if (schedule.IsDayOff)
        {
            bool isWorking = _staffSchedules.Any(s => s.ProfessionalId == professionalId && s.Day == day && !s.IsDayOff);

            if (isWorking)
            {
                throw new DomainException("Cannot create full-day off when partial schedule exists.");
            }
        }
        else
        {
            var storeDayHours = _storeSchedules.Where(h => h.Day == day).ToList();

            if (!IsOpenOnDay(day))
            {
                throw new DomainException("Cannot schedule staff on a day the store is closed.");
            }

            bool isOverlapping = _staffSchedules.Any(
                s => s.ProfessionalId == professionalId &&
                s.Day == day &&
                s.StartTime.HasValue &&
                s.EndTime.HasValue &&
                s.StartTime.Value < endTime &&
                s.EndTime.Value > startTime);

            if (isOverlapping)
            {
                throw new DomainException("Schedule overlaps with existing schedule.");
            }
        }

        _staffSchedules.Add(schedule);
        MarkAsUpdated();

        return schedule;
    }

    public void RemoveStaffSchedule(int ownerId, int scheduleId)
    {
        if (IsDeleted)
        {
            throw new DomainException("Cannot remove schedules from an inactive store.");
        }

        if (!IsOwner(ownerId))
        {
            throw new DomainException("Only an owner can remove staff schedules.");
        }

        var schedule = _staffSchedules.FirstOrDefault(s => s.Id == scheduleId);

        if (schedule == null)
        {
            throw new DomainException("Schedule does not exist.");
        }

        _staffSchedules.Remove(schedule);
        MarkAsUpdated();
    }

    public StoreProfessionalException AddStaffException(int ownerId, int professionalId, DateTime date, TimeSpan? startTime = null, TimeSpan? endTime = null, string? reason = null)
    {
        if (IsDeleted)
        {
            throw new DomainException("Cannot add staff exceptions to an inactive store.");
        }

        if (!IsOwner(ownerId))
        {
            throw new DomainException("Only an owner can add staff exceptions.");
        }

        if (!IsStaff(professionalId))
        {
            throw new DomainException("Cannot add exception for a professional who does not work at this store.");
        }

        var exception = StoreProfessionalException.Create(Id, professionalId, date, startTime, endTime, reason);

        if (exception.IsFullDayAbsent)
        {
            bool isPartiallyWorking = _professionalExceptions.Any(
                e => e.ProfessionalId == professionalId &&
                e.Date == date &&
                !e.IsFullDayAbsent
            );

            if (isPartiallyWorking)
            {
                throw new DomainException("Cannot schedule staff to partially work on a day off.");
            }
        }
        else
        {
            bool isOverlapping = _professionalExceptions.Any(
                e => e.ProfessionalId == professionalId &&
                e.Date == date &&
                e.StartTime.HasValue &&
                e.EndTime.HasValue &&
                e.StartTime.Value < endTime &&
                e.EndTime.Value > startTime
                );

            if (isOverlapping)
            {
                throw new DomainException("Exception overlaps with an existing exception for this professional.");
            }
        }

        _professionalExceptions.Add(exception);
        MarkAsUpdated();

        return exception;
    }

    public void RemoveStaffException(int ownerId, int exceptionId)
    {
        if (IsDeleted)
        {
            throw new DomainException("Cannot remove exceptions from an inactive store.");
        }

        if (!IsOwner(ownerId))
        {
            throw new DomainException("Only an owner can remove professional exceptions.");
        }

        var exception = _professionalExceptions.FirstOrDefault(e => e.Id == exceptionId);

        if (exception == null)
        {
            throw new DomainException("Professional exception not found.");
        }

        _professionalExceptions.Remove(exception);
        MarkAsUpdated();
    }

    public bool IsOpenAt(DateTime date)
    {
        var dateExceptions = _exceptions.Where(e => e.Date.Date == date.Date).ToList();

        if (dateExceptions.Any())
        {
            return dateExceptions.Any(
                e => !e.IsFullDayClosed &&
                e.OpenTime.HasValue &&
                e.CloseTime.HasValue &&
                date.TimeOfDay >= e.OpenTime.Value &&
                date.TimeOfDay < e.CloseTime.Value
            );
        }

        var dayHours = _storeSchedules.Where(h => h.Day == date.DayOfWeek).ToList();

        return dayHours.Any(
            h => !h.IsFullDayClosed &&
            h.OpenTime.HasValue &&
            h.CloseTime.HasValue &&
            date.TimeOfDay >= h.OpenTime.Value &&
            date.TimeOfDay < h.CloseTime.Value);
    }

    public bool IsOpenOnDay(DayOfWeek day)
    {
        return !_storeSchedules.Any(h => h.Day == day && h.IsFullDayClosed);
    }

    private static void ValidateStoreInfo(string name, string address, string taxIdNumber, string email, string phone)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("Name is required.");
        }

        if (name.Length > 100)
        {
            throw new DomainException("Name cannot exceed 100 characters.");
        }

        if (string.IsNullOrWhiteSpace(address))
        {
            throw new DomainException("Address is required.");
        }

        if (address.Length > 200)
        {
            throw new DomainException("Address cannot exceed 200 characters.");
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            throw new DomainException("Email is required.");
        }

        if (!IsValidEmail(email))
        {
            throw new DomainException("Invalid email format.");
        }

        if (string.IsNullOrWhiteSpace(phone))
        {
            throw new DomainException("Phone is required.");
        }

        if (phone.Length > 20)
        {
            throw new DomainException("Phone cannot exceed 20 characters.");
        }

        if (string.IsNullOrWhiteSpace(taxIdNumber))
        {
            throw new DomainException("Tax ID number is required.");
        }

        if (taxIdNumber.Length > 50)
        {
            throw new DomainException("Tax ID number cannot exceed 50 characters.");
        }
    }

    private static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
}
