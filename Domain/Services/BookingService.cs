using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;

namespace Domain.Services;

public class BookingService
{
    private readonly IStoreScheduleRepository _storeScheduleRepository;
    private readonly IStoreStaffScheduleRepository _storeStaffScheduleRepository;
    private readonly IStoreServiceRepository _storeServiceRepository;
    private readonly IProfessionalAppointmentRepository _professionalAppointmentRepository;

    public BookingService(IStoreScheduleRepository storeScheduleRepository, IStoreStaffScheduleRepository storeStaffScheduleRepository, IStoreServiceRepository storeServiceRepository, IProfessionalAppointmentRepository professionalAppointmentRepository)
    {
        _storeScheduleRepository = storeScheduleRepository;
        _storeStaffScheduleRepository = storeStaffScheduleRepository;
        _storeServiceRepository = storeServiceRepository;
        _professionalAppointmentRepository = professionalAppointmentRepository;
    }

    public async Task<Appointment> CreateAppointment(int userId, int serviceId, int professionalId, int storeId, DateTime startAt, string? notes = null)
    {
        var storeScheduleManager = _storeScheduleRepository.GetByStoreId(storeId);
        var storeStaffScheduleManager = _storeStaffScheduleRepository.GetByStoreIdAndProfessionalId(storeId, professionalId);
        var storeServiceManager = _storeServiceRepository.GetByStoreId(storeId);
        var professionalAppointmentManager = _professionalAppointmentRepository.GetByProfessionalId(professionalId);

        var service = storeServiceManager.Services.First(s => s.Id == serviceId);
        decimal price = service.Price;
        DateTime endAt = startAt.Add(service.Duration);

        if (!storeScheduleManager.IsOpenAt(startAt))
        {
            throw new DomainException("Store is closed at the requested time.");
        }

        if (!storeStaffScheduleManager.IsProfessionalAvailable(startAt, endAt))
        {
            throw new DomainException("Professional is unavailable at the requested time.");
        }

        if (!storeServiceManager.ServiceIsProvidedByProfessional(professionalId, serviceId))
        {
            throw new DomainException("Service is not offered by the professional.");
        }

        if (!storeServiceManager.ServiceIsProvidedByTheStore(serviceId))
        {
            throw new DomainException("Service is not offered by the store.");
        }

        var appointment = professionalAppointmentManager.ScheduleAppointment(userId, storeId, serviceId, price, startAt, endAt, notes);

        return appointment;
    }
}