using Application.Appointments.Common.DTO;
using Application.Appointments.Common.Queries;
using Application.Appointments.GetDetails;
using Application.Common.DTO;
using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Appointments;

public sealed class AppointmentQueries : IAppointmentQueries
{
    private readonly AppDbContext _context;

    public AppointmentQueries(AppDbContext context)
    {
        _context = context;
    }

    public async Task<AppointmentDetailsDTO?> GetAppointmentDetailsAsync(int appointmentId, CancellationToken ct)
    {
        return await (
            from appointment in _context.Appointments
            join store in _context.Stores
                on appointment.StoreId equals store.Id

            join user in _context.Users
                on appointment.UserId equals user.Id

            join professional in _context.Professionals
                on appointment.ProfessionalId equals professional.Id

            join catalog in _context.StoreCatalogs
                on appointment.StoreId equals catalog.StoreId

            from offering in catalog.Offerings
                .Where(o => o.Id == appointment.OfferingId)

            where appointment.Id == appointmentId

            select new AppointmentDetailsDTO(
                appointment.Id,

                new BasicStoreDTO(
                    store.Id,
                    store.Name.Value
                ),

                new BasicUserDTO(
                    user.Id,
                    user.FullName.FirstName,
                    user.FullName.LastName,
                    user.Email.Value
                ),

                new BasicProfessionalDTO(
                    professional.Id,
                    professional.FullName.FirstName,
                    professional.FullName.LastName,
                    professional.Email.Value
                ),

                new BasicOfferingDTO(
                    offering.Id,
                    offering.Name.Value,
                    offering.Price.Amount,
                    offering.Price.Currency,
                    offering.Duration.Minutes
                ),

                appointment.StartAt.Value,
                appointment.EndAt.Value,
                appointment.Status.ToString(),
                appointment.Notes.Value
            )
        ).FirstOrDefaultAsync(ct);
    }

    public async Task<PagedResult<AppointmentListItemDTO>> GetProfessionalAppointmentsAsync(
        int professionalId,
        DateOnly? from,
        DateOnly? to,
        int? page,
        int? pageSize,
        CancellationToken ct
    )
    {
        var query = AppointmentListQuery(from, to)
            .Where(a => a.ProfessionalId == professionalId);

        var totalCount = await query.CountAsync(ct);

        IReadOnlyCollection<AppointmentListItemDTO> items;

        if (page.HasValue && pageSize.HasValue)
        {
            var offset = (page.Value - 1) * pageSize.Value;
            items = await query
                .Skip(offset)
                .Take(pageSize.Value)
                .ToListAsync(ct);
        }
        else
        {
            items = await query.ToListAsync(ct);
        }

        return new PagedResult<AppointmentListItemDTO>(
            items,
            page ?? 1,
            pageSize ?? totalCount,
            totalCount
        );
    }

    public async Task<PagedResult<AppointmentListItemDTO>> GetStoreAppointmentsAsync(int storeId, DateOnly? from, DateOnly? to, int? page, int? pageSize, CancellationToken ct)
    {
        var query = AppointmentListQuery(from, to)
            .Where(a => a.StoreId == storeId);

        var totalCount = await query.CountAsync(ct);

        IReadOnlyCollection<AppointmentListItemDTO> items;

        if (page.HasValue && pageSize.HasValue)
        {
            var offset = (page.Value - 1) * pageSize.Value;
            items = await query
                .Skip(offset)
                .Take(pageSize.Value)
                .ToListAsync(ct);
        }
        else
        {
            items = await query.ToListAsync(ct);
        }

        return new PagedResult<AppointmentListItemDTO>(
            items,
            page ?? 1,
            pageSize ?? totalCount,
            totalCount
        );
    }

    public async Task<PagedResult<AppointmentListItemDTO>> GetUserAppointmentsAsync(int userId, DateOnly? from, DateOnly? to, int? page, int? pageSize, CancellationToken ct)
    {
        var query = AppointmentListQuery(from, to)
            .Where(a => a.UserId == userId);

        var totalCount = await query.CountAsync(ct);

        IReadOnlyCollection<AppointmentListItemDTO> items;

        if (page.HasValue && pageSize.HasValue)
        {
            var offset = (page.Value - 1) * pageSize.Value;
            items = await query.Skip(offset)
                .Take(pageSize.Value)
                .ToListAsync(ct);
        }
        else
        {
            items = await query.ToListAsync(ct);
        }

        return new PagedResult<AppointmentListItemDTO>(
            items,
            page ?? 1,
            pageSize ?? totalCount,
            totalCount
        );
    }

    private IQueryable<AppointmentListItemDTO> AppointmentListQuery(
            DateOnly? from,
            DateOnly? to)
    {
        var fromDateTime = from?.ToDateTime(TimeOnly.MinValue);
        var toDateTime = to?.ToDateTime(TimeOnly.MaxValue);

        return
            from appointment in _context.Appointments

            join store in _context.Stores
                on appointment.StoreId equals store.Id

            join user in _context.Users
                on appointment.UserId equals user.Id

            join professional in _context.Professionals
                on appointment.ProfessionalId equals professional.Id

            join catalog in _context.StoreCatalogs
                on appointment.StoreId equals catalog.StoreId

            from offering in catalog.Offerings
                .Where(o => o.Id == appointment.OfferingId)

            where (!toDateTime.HasValue || appointment.StartAt.Value < toDateTime)
               &&
                (!fromDateTime.HasValue || appointment.EndAt.Value > fromDateTime)

            orderby appointment.StartAt.Value

            select new AppointmentListItemDTO(
                appointment.Id,

                store.Id,
                store.Name.Value,

                user.Id,
                user.FullName.ToString(),

                professional.Id,
                professional.FullName.ToString(),

                offering.Id,
                offering.Name.Value,

                appointment.StartAt.Value,
                appointment.EndAt.Value,

                appointment.Price.Amount,
                appointment.Price.Currency,

                appointment.Status.ToString(),
                appointment.Notes.Value
            );
    }

}


