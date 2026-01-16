using Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Filters;

internal static class SoftDeleteFilters
{
    public static void ApplySoftDeleteFilter<TEntity>(ModelBuilder modelBuilder) where TEntity : HistoricEntity
    {
        modelBuilder.Entity<TEntity>().HasQueryFilter(e => e.DeletedAt == null);
    }
}