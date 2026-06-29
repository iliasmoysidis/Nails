using Application.Appointments.Common.Repositories;
using Application.Assignments.Common.Repositories;
using Application.Calendar.Common.Repositories;
using Application.Catalog.Common.Repositories;
using Application.Professionals.Common.Repositories;
using Application.Schedule.Common.Repositories;
using Application.Stores.Common.Repositories;
using Application.Users.Common.Repositories;
using Domain.UserSchedules;
using Domain.Appointments.Services;
using Domain.Professionals;
using Domain.Stores;
using Domain.Users;
using Application.Common.Abstractions.Context;
using Application.Common.Exceptions;

namespace Application.Appointments.Create;

public sealed class Loader
    : IRequestContextLoader<Command, Context>
{
    private readonly IUserRepository _userRepo;
    private readonly IStoreRepository _storeRepo;
    private readonly IProfessionalRepository _professionalRepo;
    private readonly IAppointmentRepository _appointmentRepo;
    private readonly IStoreCalendarRepository _storeCalendarRepo;
    private readonly IProfessionalScheduleRepository _professionalScheduleRepo;
    private readonly IStoreCatalogRepository _storeCatalogRepo;
    private readonly IAssignmentRegistryRepository _assignmentRegistryRepo;

    public Loader(
        IUserRepository userRepo,
        IStoreRepository storeRepo,
        IProfessionalRepository professionalRepo,
        IStoreCatalogRepository storeCatalogRepo,
        IProfessionalScheduleRepository professionalScheduleRepo,
        IStoreCalendarRepository storeCalendarRepo,
        IAssignmentRegistryRepository assignmentRegistryRepo,
        IAppointmentRepository appointmentRepo
    )
    {
        _userRepo = userRepo;
        _storeRepo = storeRepo;
        _professionalRepo = professionalRepo;
        _storeCatalogRepo = storeCatalogRepo;
        _professionalScheduleRepo = professionalScheduleRepo;
        _storeCalendarRepo = storeCalendarRepo;
        _assignmentRegistryRepo = assignmentRegistryRepo;
        _appointmentRepo = appointmentRepo;
    }

    public async Task PopulateAsync(
        Command command,
        Context ctx,
        CancellationToken ct)
    {
        var user = await _userRepo.GetByIdAsync(command.UserId, ct)
            ?? throw new ApplicationLayerNotFoundException("User not found.");

        var store = await _storeRepo.GetByIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Store not found");

        var professional = await _professionalRepo.GetByIdAsync(command.ProfessionalId, ct)
            ?? throw new ApplicationLayerNotFoundException("Professional not found.");

        var storeCatalog = await _storeCatalogRepo.GetByIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Store catalog not found");

        var storeCalendar = await _storeCalendarRepo.GetByIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Store calendar not found");

        var professionalSchedule = await _professionalScheduleRepo.GetByProfessionalIdAsync(command.ProfessionalId, ct)
            ?? throw new ApplicationLayerNotFoundException("Professional schedule not found");

        var staffCalendar = professionalSchedule.Calendars.FirstOrDefault(c => c.StoreId == command.StoreId)
            ?? throw new ApplicationLayerNotFoundException("Professional calendar not found");

        var assignments = await _assignmentRegistryRepo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Assignments not found");

        var professionalAppointments = await _appointmentRepo.GetUpcomingByProfessionalIdAsync(command.ProfessionalId, ct);

        var userAppointments = await _appointmentRepo.GetByUserIdAsync(command.UserId, ct);

        ctx.User = user;

        ctx.AppointmentBooking = new AppointmentBooking(
            professional: professional,
            store: store,
            storeCalendar: storeCalendar,
            staffCalendar: staffCalendar,
            storeCatalog: storeCatalog,
            assignments: assignments,
            appointments: professionalAppointments
        );

        ctx.UserSchedule = new UserSchedule(
            userId: user.Id,
            appointments: userAppointments
        );
    }
}
